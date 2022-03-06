using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.HR;
using core.Entities.Identity;
using core.Entities.MasterEntities;
using core.Entities.Orders;
using core.Interfaces;
using core.ParamsAndDtos;
using infra.Data;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class ChecklistService : IChecklistService
     {
        private readonly ATSContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeService _empService;
        private readonly ICVReviewService _cvReviewService;
        private readonly IUserService _userService;
        private readonly ICommonServices _commonService;
        public ChecklistService(ATSContext context, IUnitOfWork unitOfWork, IUserService userService,
            IEmployeeService empService, ICVReviewService cvReviewService, 
            ICommonServices commonService)
        {
            _commonService = commonService;
            _userService = userService;
            _cvReviewService = cvReviewService;
            _empService = empService;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        
        private async Task<ChecklistHR> AddChecklistHR(int candidateid, int orderitemid, int employeeid)
        {
            var itemList = new List<ChecklistHRItem>();
            //populate the checklistHRItem
            var data = await _context.ChecklistHRDatas.OrderBy(x => x.SrNo).ToListAsync();
            
            foreach (var item in data)
            {
                itemList.Add(new ChecklistHRItem(item.SrNo, item.Parameter));
            }
            var hrTask = new ChecklistHR(candidateid, orderitemid, employeeid, System.DateTime.Now, itemList);

            _unitOfWork.Repository<ChecklistHR>().Add(hrTask);

            if (await _unitOfWork.Complete() == 0) throw new Exception("Failed to save the Checklist details");
            return hrTask;
        }
        public async Task<ChecklistHR> AddNewChecklistHR(int candidateId, int orderItemId, LoggedInUserDto loggedInUserDto)
        {
            //check if the candidate has aleady been checklisted for the order item
            var checkedOn = await _context.ChecklistHRs.Where(x => x.CandidateId == candidateId && x.OrderItemId == orderItemId)
                .Select(x => x.CheckedOn.Date).FirstOrDefaultAsync();
            if (checkedOn.Year > 2000) throw new Exception("Checklist on the candidate for the same requirement has been done on " + checkedOn);
                
            //update loggerInUserDto.LoggedInEmployeeId
            if(loggedInUserDto.LoggedInEmployeeId == 0) loggedInUserDto.LoggedInEmployeeId = await _empService.GetEmployeeIdFromAppUserIdAsync(loggedInUserDto.LoggedInAppUserId);

            var hr = await AddChecklistHR(candidateId, orderItemId, loggedInUserDto.LoggedInEmployeeId);
            
            return hr;
        }

        public async Task<bool> EditChecklistHR(ChecklistHRDto model, LoggedInUserDto loggedInUserDto)
        {
            var dto = await GetChecklistHRIfEditable(model, loggedInUserDto);     //returns ChecklistHR
            if (!string.IsNullOrEmpty(dto.ErrorDesc)) throw new System.Exception(dto.ErrorDesc);

            var existing = dto.ChecklistHR;
            _context.Entry(existing).CurrentValues.SetValues(model);   //saves only the parent, not children

            //the children 
            //Delete children that exist in existing record, but not in the new model order
            foreach (var existingItem in existing.ChecklistHRItems.ToList())
            {
                if (!model.ChecklistHR.ChecklistHRItems.Any(c => c.Id == existingItem.Id && c.Id != default(int)))
                {
                    _context.ChecklistHRItems.Remove(existingItem);
                    _context.Entry(existingItem).State = EntityState.Deleted;
                }
            }

            //children that are not deleted, are either updated or new ones to be added
            foreach (var modelItem in model.ChecklistHR.ChecklistHRItems)
            {
                var existingItem = existing.ChecklistHRItems.Where(c => c.Id == modelItem.Id && c.Id != default(int)).SingleOrDefault();
                if (existingItem != null)       // Update child
                {
                    _context.Entry(existingItem).CurrentValues.SetValues(modelItem);
                    _context.Entry(existingItem).State = EntityState.Modified;
                }
                else            //insert children as new record
                {
                    var newItem = new ChecklistHRItem(modelItem.SrNo, modelItem.Parameter, modelItem.Response, modelItem.Exceptions);
                    existing.ChecklistHRItems.Add(newItem);
                    _context.Entry(newItem).State = EntityState.Added;
                }
            }
            _context.Entry(existing).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteChecklistHR(ChecklistHRDto checklistHR, LoggedInUserDto loggedInDto)
        {
            var obj = await GetChecklistHRIfEditable(checklistHR, loggedInDto);
            if (!string.IsNullOrEmpty(obj.ErrorDesc)) throw new System.Exception(obj.ErrorDesc);

            _unitOfWork.Repository<ChecklistHR>().Delete(checklistHR.ChecklistHR);

            return await _unitOfWork.Complete() > 0;
        }

        public async Task<ChecklistDto> GetChecklistHR(int candidateId, int orderItemId, LoggedInUserDto loggedInUserDto)
        {
            var lst = await _context.ChecklistHRs.Where(x => x.CandidateId == candidateId &&
                x.OrderItemId == orderItemId)       //  && x.UserId == loggedInUserDto.LoggedInEmployeeId)
                .Include(x => x.ChecklistHRItems).FirstOrDefaultAsync();
            
            if (lst == null) {
                lst = await AddChecklistHR(candidateId, orderItemId, loggedInUserDto?.LoggedInEmployeeId ?? 0);
            }
            var cv = await _userService.GetCandidateBriefById(candidateId);
            var item = await _commonService.CategoryRefFromOrderItemId(orderItemId);
                    
            var dto = new ChecklistDto{
                Id = lst.Id, ApplicationNo = cv.ApplicationNo, OrderItemId = orderItemId,
                CategoryRef = item, CandidateName = cv.FullName, CandidateId = cv.Id,
                OrderRef =  item, UserLoggedIn = "Not defined",
                CheckedOn = lst.CheckedOn, HrExecComments = lst.HrExecComments,
                ChecklistHRItems = lst.ChecklistHRItems
            };

            return dto;
        }
        
        private async Task<ChecklistHRDto> GetChecklistHRIfEditable(ChecklistHRDto model, LoggedInUserDto loggedInDto)
        {
            //if cv already forwarded to Sup, then changes not allowed
            var existing = _context.ChecklistHRs.Where(p => p.Id == model.ChecklistHR.Id)
                .Include(p => p.ChecklistHRItems)
                .AsNoTracking()
                .SingleOrDefault();

            if (existing == null)
            {
                throw new Exception("Checklist record you want edited does not exist");

                /* if (loggedInDto.LoggedInEmployeeId == 0) loggedInDto.LoggedInEmployeeId = await _empService.GetEmployeeIdFromAppUserIdAsync(loggedInDto.LoggedInAppUserId);
                var hrexecid = await _context.OrderItems.Where(x => x.Id == model.OrderItemId).Select(x => x.HrExecId).FirstOrDefaultAsync();
                if (model.CandidateId==0 || model.OrderItemId == 0) throw new Exception("Candidate or Order Item detail not provided");
                if (hrexecid != loggedInDto.LoggedInEmployeeId) throw new Exception("The LoggedIn user should be the one tasked to work on this category");
                existing = await CreateChecklistHR(isEdit, model.CandidateId, model.OrderItemId, autoSubmit, loggedInDto);
                */
            }

            var dto = new ChecklistHRDto();

            var submitted = await _context.CVReviews
                .Where(x => x.CandidateId == model.ChecklistHR.CandidateId && x.OrderItemId == model.ChecklistHR.OrderItemId 
                    && x.SubmittedByHRExecOn.Year > 2000)
                .Select(x => x.SubmittedByHRExecOn)
                .FirstOrDefaultAsync();

            if (submitted.Year > 2000)
            {
                dto.ErrorDesc = "This Checklist is referred by the HR Executive on " + submitted.Date + " and cannot be edited now";
                return dto;
            }
            
            /*if (existing.UserId != model.UserId)
            {
                dto.ErrorDesc = "Only the user that conducted the checklist can edit it";
            }
            else */
            if (existing == null)
            {
                dto.ErrorDesc = "No such record exist in database";
            }
            else
            {
                dto.ChecklistHR = existing;
            }

            return dto;
        }


        //master data
        public async Task<ChecklistHRData> AddChecklistHRParameter(string checklistParameter)
        {
            var srno = await _context.ChecklistHRDatas.MaxAsync(x => x.SrNo) + 1;
            var checklist = new ChecklistHRData(srno, checklistParameter);
            _unitOfWork.Repository<ChecklistHRData>().Add(checklist);
            if (await _unitOfWork.Complete() > 0) return checklist;
            return null;
        }

        public async Task<bool> DeleteChecklistHRDataAsync(ChecklistHRData checklistHRData)
        {
            _unitOfWork.Repository<ChecklistHRData>().Delete(checklistHRData);
            return (await _unitOfWork.Complete() > 0);
        }
        public async Task<bool> EditChecklistHRDataAsync(ChecklistHRData checklistHRData)
        {
            _unitOfWork.Repository<ChecklistHRData>().Update(checklistHRData);
            return (await _unitOfWork.Complete() > 0);
        }

        public Task<IReadOnlyList<ChecklistHRData>> GetChecklistHRDataListAsync()
        {
            throw new System.NotImplementedException();
        }

        private async Task<bool> AutoSubmitCVReview(int candidateid, int orderitemid, int checklisthrid, LoggedInUserDto loggedInUserDto)
        {
            var hrexectask = await _context.Tasks.Where(x => x.OrderItemId == orderitemid &&
                x.AssignedToId == loggedInUserDto.LoggedInEmployeeId && 
                x.TaskTypeId == (int)EnumTaskType.AssignTaskToHRExec).FirstOrDefaultAsync();
            var orderitem = await _context.OrderItems.Where(x => x.Id == orderitemid)
                .Select(x => new { x.HrSupId, x.Charges, x.NoReviewBySupervisor }).FirstOrDefaultAsync();

            var cv = new CVReviewBySupDto
            {
                    ChecklistHRId = checklisthrid,
                    //TaskId = hrexectask.Id,
                    enumTaskType = core.Entities.Orders.EnumTaskType.SubmitCVToHRSupForReview,
                    TaskOwnerId = hrexectask.TaskOwnerId,
                    AssignedToId = (int)orderitem.HrSupId,
                    NoReviewBySupervisor = orderitem.NoReviewBySupervisor,
                    Charges = orderitem.Charges,
                    OrderItemId = orderitemid,
                    CandidateId = candidateid
            };

            var cvdto = new List<CVReviewBySupDto>();
            cvdto.Add(cv);
            var msgs = await _cvReviewService.CVReviewByHRSup(loggedInUserDto, cvdto);
            if (msgs == null || msgs.Count == 0) throw new Exception("Checklist for HR created, but failed to submit the CV to the Supervisor");
            
            return true;
        }

         
     }
}