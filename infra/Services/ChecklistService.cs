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
        public ChecklistService(ATSContext context, IUnitOfWork unitOfWork, IEmployeeService empService, ICVReviewService cvReviewService)
        {
            _cvReviewService = cvReviewService;
            _empService = empService;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<ChecklistHR> AddNewChecklistHR(int candidateId, int orderItemId, LoggedInUserDto loggedInUserDto)
        {
            //check if the candidate has aleady been checklisted for the order item
            var checkedOn = await _context.ChecklistHRs.Where(x => x.CandidateId == candidateId && x.OrderItemId == orderItemId)
                .Select(x => x.CheckedOn.Date).FirstOrDefaultAsync();
            if (checkedOn != null && checkedOn.Year > 2000) throw new Exception("Checklist on the candidate for the same requirement has been done on " + checkedOn);
                
            //update loggerInUserDto.LoggedInEmployeeId
            if(loggedInUserDto.LoggedInEmployeeId == 0) loggedInUserDto.LoggedInEmployeeId = await _empService.GetEmployeeIdFromAppUserIdAsync(loggedInUserDto.LoggedInAppUserId);

            //populate the checklistHRItem
            var data = await _context.ChecklistHRDatas.OrderBy(x => x.SrNo).ToListAsync();
            var itemList = new List<ChecklistItemHR>();
            foreach (var item in data)
            {
                itemList.Add(new ChecklistItemHR(item.SrNo, item.Parameter));
            }
            var hr = new ChecklistHR(candidateId, orderItemId, loggedInUserDto.LoggedInEmployeeId, System.DateTime.Now, itemList);

            _unitOfWork.Repository<ChecklistHR>().Add(hr);

            if (await _unitOfWork.Complete() == 0) throw new Exception("Failed to save the Checklist details");

            return hr;
        }

        public async Task<bool> EditChecklistHR(ChecklistHR model, LoggedInUserDto loggedInUserDto)
        {
            var dto = await GetChecklistHRIfEditable(model, loggedInUserDto);     //returns ChecklistHR
            if (!string.IsNullOrEmpty(dto.ErrorDesc)) throw new System.Exception(dto.ErrorDesc);

            var existing = dto.ChecklistHR;
            _context.Entry(existing).CurrentValues.SetValues(model);   //saves only the parent, not children

            //the children 
            //Delete children that exist in existing record, but not in the new model order
            foreach (var existingItem in existing.ChecklistItemHRs.ToList())
            {
                if (!model.ChecklistItemHRs.Any(c => c.Id == existingItem.Id && c.Id != default(int)))
                {
                    _context.CheckListItemHRs.Remove(existingItem);
                    _context.Entry(existingItem).State = EntityState.Deleted;
                }
            }

            //children that are not deleted, are either updated or new ones to be added
            foreach (var modelItem in model.ChecklistItemHRs)
            {
                var existingItem = existing.ChecklistItemHRs.Where(c => c.Id == modelItem.Id && c.Id != default(int)).SingleOrDefault();
                if (existingItem != null)       // Update child
                {
                    _context.Entry(existingItem).CurrentValues.SetValues(modelItem);
                    _context.Entry(existingItem).State = EntityState.Modified;
                }
                else            //insert children as new record
                {
                    var newItem = new ChecklistItemHR(modelItem.SrNo, modelItem.Parameter, modelItem.Response, modelItem.Exceptions);
                    existing.ChecklistItemHRs.Add(newItem);
                    _context.Entry(newItem).State = EntityState.Added;
                }
            }
            _context.Entry(existing).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteChecklistHR(ChecklistHR checklistHR, LoggedInUserDto loggedInDto)
        {
            var obj = await GetChecklistHRIfEditable(checklistHR, loggedInDto);
            if (!string.IsNullOrEmpty(obj.ErrorDesc)) throw new System.Exception(obj.ErrorDesc);

            _unitOfWork.Repository<ChecklistHR>().Delete(checklistHR);

            return await _unitOfWork.Complete() > 0;
        }

        public async Task<ChecklistHR> GetChecklistHR(int candidateId, int orderItemId, LoggedInUserDto loggedInUserDto)
        {
            return await _context.ChecklistHRs.Where(x => x.CandidateId == candidateId &&
                x.OrderItemId == orderItemId  && x.UserId == loggedInUserDto.LoggedInEmployeeId)
                .Include(x => x.ChecklistItemHRs).FirstOrDefaultAsync();
        }
        
        private async Task<ChecklistHRDto> GetChecklistHRIfEditable(ChecklistHR model, LoggedInUserDto loggedInDto)
        {
            //if cv already forwarded to Sup, then changes not allowed
            var existing = _context.ChecklistHRs.Where(p => p.Id == model.Id)
                .Include(p => p.ChecklistItemHRs)
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
                .Where(x => x.CandidateId == model.CandidateId && x.OrderItemId == model.OrderItemId 
                    && x.SubmittedByHRExecOn.Year > 2000)
                .Select(x => x.SubmittedByHRExecOn)
                .FirstOrDefaultAsync();

            if (submitted != null && submitted.Year > 2000)
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