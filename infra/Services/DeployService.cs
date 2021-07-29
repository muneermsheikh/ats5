using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.HR;
using core.Entities.Process;
using core.Interfaces;
using core.ParamsAndDtos;
using infra.Data;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class DeployService : IDeployService
     {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ATSContext _context;
        public DeployService(IUnitOfWork unitOfWork, ATSContext context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<CommonDataDto>> GetPendingDeployments()
        {
            var tempQuery =  from d in _context.Deploys
                group d by d.CVRefId into dTop 
                orderby dTop.Key descending
                select new {
                        Key = dTop.First(),
                        Status = dTop.First()
                };

            var qry = await (from r in _context.CVRefs 
                join d in tempQuery on r.Id equals d.Key.CVRefId
                join i in _context.OrderItems on r.OrderItemId equals i.Id 
                join cat in _context.Categories on i.CategoryId equals cat.Id
                join ordr in _context.Orders on i.OrderId equals ordr.Id 
                join c in _context.Customers on ordr.CustomerId equals c.Id
                join cand in _context.Candidates on r.CandidateId equals cand.Id
                select (new CommonDataDto {
                        ApplicationNo = cand.ApplicationNo,
                        CandidateName = cand.FullName,
                        CustomerName = c.CustomerName, 
                        CategoryName = cat.Name, 
                        OrderNo = ordr.OrderNo,
                        DeployStatus = d.Status.StatusId,
                        DeployStatusDate =d.Status.TransactionDate // DateTime.ParseExact(d.Status.TransactionDate, "yyyy/MM/DD", CultureInfo.InvariantCulture);
                })).ToListAsync();

               return qry;
        }
        
        public async Task<bool> AddDeploymentTransaction(Deploy deploy)
        {
            if (await DeploymentStageInSequence(deploy) != null) {
                return false;
            }

            _unitOfWork.Repository<Deploy>().Add(deploy);
            return await _unitOfWork.Complete() > 0;
        }

        public async Task<bool> DeleteDeploymentTransactions(Deploy deploy)
        {
            _unitOfWork.Repository<Deploy>().Delete(deploy);
            return await _unitOfWork.Complete() > 0;
        }

        public async Task<bool> EditDeploymentTransaction(Deploy deploy)
        {
            _unitOfWork.Repository<Deploy>().Update(deploy);
            return await _unitOfWork.Complete() > 0;
        }

        public async Task<CVRef> GetDeploymentsByCandidateAndOrderItem(int candidateId, int orderItemId)
        {
            return await _context.CVRefs.Where(x => x.CandidateId == candidateId && x.OrderItemId == orderItemId)
            .Include(x => x.Deploys.OrderByDescending(x => x.TransactionDate)).FirstOrDefaultAsync();
        }

        public async Task<CVRef> GetDeploymentsById(int cvrefid)
        {
            return await _context.CVRefs.Where(x => x.Id == cvrefid)
            .Include(x => x.Deploys.OrderByDescending(x => x.TransactionDate)).FirstOrDefaultAsync();
        }

        public async Task<ICollection<CVRef>> GetDeploymentsOfACandidate(int candidateId)
        {
            return await _context.CVRefs.Where(x => x.CandidateId == candidateId)
            .Include(x => x.Deploys.OrderByDescending(x => x.TransactionDate)).ToListAsync();
        }

        public async Task<ICollection<CVRef>> GetDeploymentsOfOrderItemId(int orderItemId)
        {
            return await _context.CVRefs.Where(x => x.OrderItemId == orderItemId)
            .Include(x => x.Deploys.OrderByDescending(x => x.TransactionDate)).ToListAsync();
        }

        private async Task<Deploy> DeploymentStageInSequence(Deploy deploy)
        {
            //check if deployments concluded, if so no deployments allowed
            var lastStatus = await _context.Deploys.Where(x => x.CVRefId == deploy.CVRefId)
                .MaxAsync(x => x.StatusId);
            if ((EnumDeployStatus)lastStatus == EnumDeployStatus.Concluded) {
                return null;
            }

            var newDeployStatusId = deploy.StatusId;

            bool ok=false;
            switch((EnumDeployStatus)newDeployStatusId)     //following actions are not relevant to the process, hence can be out of sequence
            {
                case EnumDeployStatus.OfferLetterAccepted:
                    ok=true;
                    break;
                case EnumDeployStatus.VisaQueryRaised:
                    ok = true;
                    break;
                case EnumDeployStatus.EmigrationQueryRaised:
                    ok = true;
                    break;
                default:
                    break;
            }

            if(!ok) return null;

            var lastDeployStageId = await _context.Deploys
                .Where(x => x.CVRefId == deploy.CVRefId)
                .MaxAsync(x => x.StatusId);
            
            var dep = await _context.Deploys.Where(x => x.CVRefId == deploy.CVRefId)
                .OrderByDescending(x => x.StatusId).Take(1).FirstOrDefaultAsync();
           
            if (dep.StatusId < lastDeployStageId) return null;
            
            if (dep.NextStatusId == 0) {
                var nextstatus = await _context.DeployStatus.Where(x => x.StageId == dep.StatusId)
                    .Select(x => new {x.NextStageId, x.WorkingDaysReqdForNextStage}).FirstOrDefaultAsync();
                deploy.NextStatusId = nextstatus.NextStageId;    
                deploy.NextEstimatedStatusDate = DateTime.Today.AddDays(nextstatus.WorkingDaysReqdForNextStage);
                //TODO - add working days instead of days
            }

            return deploy;
        }
     }
}