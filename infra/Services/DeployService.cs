using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.HR;
using core.Entities.Process;
using core.Interfaces;
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

        public async Task<bool> AddDeploymentTransaction(Deploy deploy)
        {
            if (! await DeploymentStageInSequence(deploy)) {
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

        private async Task<bool> DeploymentStageInSequence(Deploy deploy)
        {
            //check if deployments concluded, if so no deployments allowed
            var lastStatus = await _context.Deploys.Where(x => x.CVRefId == deploy.CVRefId)
                .MaxAsync(x => x.DeployStatusId);
            if ((EnumDeployStatus)lastStatus == EnumDeployStatus.Concluded) {
                return false;
            }

            var newDeployStatusId = deploy.DeployStatusId;
            var newDeployStageId = deploy.DeployStageId;

            switch((EnumDeployStatus)newDeployStatusId)     //following actions are not relevant to the process, hence can be out of sequence
            {
                case EnumDeployStatus.OfferLetterAccepted:
                    return true;
                case EnumDeployStatus.VisaQueryRaised:
                    return true;
                case EnumDeployStatus.EmigrationQueryRaised:
                    return true;
                default:
                    break;
            }

            var lastDeployStageId = await _context.Deploys
                .Where(x => x.CVRefId == deploy.CVRefId)
                .MaxAsync(x => x.DeployStageId);

            return newDeployStageId > lastDeployStageId;
        }
     }
}