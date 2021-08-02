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

// TODO - constants for deploymentStageIds
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
                        DeployStageId = d.Status.StageId,
                        DeployStageDate =d.Status.TransactionDate // DateTime.ParseExact(d.Status.TransactionDate, "yyyy/MM/DD", CultureInfo.InvariantCulture);
                })).ToListAsync();

               return qry;
        }
        
        public async Task<bool> AddDeploymentTransaction(Deploy deploy)
        {
            if (deploy.TransactionDate.Year < 2000 ) deploy.TransactionDate = DateTime.Now;
            deploy = await DeploymentStageInSequence(deploy);
            if (deploy == null)  return false;

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
            var lastStatusStageId = await _context.Deploys.Where(x => x.CVRefId == deploy.CVRefId)
                .MaxAsync(x => (int?)x.StageId) ?? 0;
            //var nextQuestionNo = await _context.AssessmentQsBank.MaxAsync(x => (int?)x.QNo) ?? 1;

            if ((EnumDeployStatus)lastStatusStageId == EnumDeployStatus.Concluded) return null;

            //var newDeployStatusId = deploy.StatusId;
            //  todo - return null if flg is out of sequence
            bool ok=false;
            int iStageId = 0;
            int iNextStageId = 0;
            switch (deploy.StageId) 
            {      //normally, next stage is not given but calculated. when it is, it is a status out of sequence, such as queries
                      //following actions are not relevant to the process, hence can be out of sequence
                
                case (int) EnumDeployStatus.OfferLetterAccepted:
                    deploy.NextStageId = (int)EnumDeployStatus.ReferredForMedical;
                    ok=true;
                    break;
                case (int)EnumDeployStatus.VisaQueryRaised:
                    deploy.NextStageId=(int)EnumDeployStatus.VisaReceived;
                    ok = true;
                    break;
                case (int)EnumDeployStatus.EmigrationQueryRaised:
                    deploy.NextStageId=(int)EnumDeployStatus.EmigrationGranted;
                    ok = true;
                    break;
                default:
                    break;
            }
            if(ok) {
                    var iDays=await _context.DeployStatus.Where(x => x.StageId==deploy.NextStageId).Select(x => x.WorkingDaysReqdForNextStage).FirstOrDefaultAsync();
                    deploy.TransactionDate=DateTime.Today.AddDays(iDays);
                    return deploy;
            } 
                //since status id is blank, offer next logical stages
            var dep = await _context.Deploys.Where(x => x.CVRefId == deploy.CVRefId)
                .OrderByDescending(x => x.StageId).Take(1).FirstOrDefaultAsync();
            if (dep == null) {
                if (deploy.StageId != (int) EnumDeployStatus.Selected) {
                    deploy = null;
                    throw new Exception("Candidate not yet selected");
                } 
                iStageId = (int)EnumDeployStatus.Selected;
                iNextStageId = (int)EnumDeployStatus.ReferredForMedical;
            } else {
                iStageId = await _context.DeployStatus.Where(x => x.StageId == dep.StageId).Select(x => x.NextStageId).FirstOrDefaultAsync();
                iNextStageId = await _context.DeployStatus.Where(x => x.StageId == iStageId).Select(x => x.NextStageId).FirstOrDefaultAsync();
            }
            var DaysToAdd = await _context.DeployStatus.Where(x => x.StageId==iStageId)
                .Select(x => x.WorkingDaysReqdForNextStage).FirstOrDefaultAsync();
                //TODO - add working days instead of days
            deploy.StageId = iStageId;
            deploy.NextStageId = iNextStageId;
            deploy.NextEstimatedStageDate = deploy.TransactionDate.AddDays(DaysToAdd);
            return deploy;
        }
     }
}