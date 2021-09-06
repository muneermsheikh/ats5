using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using core.Entities.HR;
using core.Entities.Orders;
using core.Entities.Process;
using core.Entities.Tasks;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;
using Microsoft.EntityFrameworkCore;


// TODO - constants for deploymentStageIds
namespace infra.Services
{
     public class DeployService : IDeployService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ATSContext _context;
          private readonly IMapper _mapper;
          private readonly ICommonServices _commonServices;
          public DeployService(IUnitOfWork unitOfWork, ATSContext context, IMapper mapper, ICommonServices commonServices)
          {
               _commonServices = commonServices;
               _mapper = mapper;
               _context = context;
               _unitOfWork = unitOfWork;
          }

          public async Task<IReadOnlyList<DeploymentPendingDto>> GetPendingDeployments()
          {
               //var specs = new CVRefSpecs(pageIndex, pageSize);
               //var pendings = await _unitOfWork.Repository<CVRef>().ListAsync(specs);
               //var pendingToReturn = _mapper.Map<List<CVRef>, List<DeploymentPendingDto>>(pendings.ToList());
               //return pendingToReturn;
               
               /* var qry = from c in _context.Deploys where c.StageId < EnumDeployStatus.Concluded
                        orderby c.CVRefId, c.StageId
                        group c.CVRefId by c.CVRefId ;
               */
               var fromCVRef = await  _context.CVRefs.Where(x => x.DeployStageId < EnumDeployStatus.Concluded && 
                    x.DeployStageId > EnumDeployStatus.None).Select(x => new {
                         x.Id, x.OrderNo, x.ApplicationNo, x.CandidateId, x.CandidateName, x.CustomerName,
                         x.CategoryName, x.DeployStageId, x.Ecnr, x.ReferredOn, x.DeployStageDate
                    }).OrderByDescending(x => x.OrderNo).ToListAsync();
               
               //TODO - this is most inefficient and dangerous query, a trip to teh DB for each record in fromcVRef
               //till this is resolved, consider omitting fields from deploys
               var lst = new List<DeploymentPendingDto>();
               foreach(var c in fromCVRef)
               {
                    var dep = await _context.Deploys.Where(x => x.CVRefId == c.Id && x.StageId == c.DeployStageId)
                         .Select(x => new {x.NextStageId, x.NextEstimatedStageDate}).FirstOrDefaultAsync();
                    if (dep != null)
                    {
                         lst.Add(new DeploymentPendingDto{
                              CVRefId = c.Id,
                              ApplicationNo = c.ApplicationNo,
                              CandidateName = c.CandidateName,
                              CategoryName = c.CategoryName,
                              CustomerName  = c.CustomerName,
                              DeployStageId = c.DeployStageId == null ? EnumDeployStatus.None : (EnumDeployStatus)c.DeployStageId,
                              DeployStageDate= c.DeployStageDate == null ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(c.DeployStageDate).Date,
                              NextStageId = dep.NextStageId,
                              NextEstimatedStageDate= dep.NextEstimatedStageDate,
                              OrderNo=c.OrderNo
                         });
                    }
               } 
               return lst;
          }

          public async Task<int> CountOfPendingDeployments()
          {
               return await _context.CVRefs.Where(x => x.DeployStageId < EnumDeployStatus.Concluded).CountAsync();
          }
          public async Task<DeployAddedDto> AddDeploymentTransaction(int cvrefId, int loggedInEmployeeId, EnumDeployStatus stageId, DateTime? transDate)
          {
               // A - if transDate missing, make it current date
               // B - Create a model based on input parameters,
               // C - verify stageId sequence, and update NextSTageId and NextSTageEstiamted date values
               // D - update DB
               // E - update CVRef.DeployStage values
               // F - Issue tasks for next process

               DateTime dt;

               // A - 
               if ((transDate.HasValue && Convert.ToDateTime(transDate).Year < 2000) || !transDate.HasValue)
               {
                    dt = DateTime.Now;
               }
               else
               {
                    dt = Convert.ToDateTime(transDate);
               }

               // B -
               var deploy = new Deploy(cvrefId, dt, stageId);

               // C - verify stageId sequence, and update NextSTageId and NextSTageEstiamted date values
               deploy = await VerifyModelStageInSeqAndUpdateNextStage(deploy);       //also updates nexTStageId and dAte
               if (deploy == null) return null;

               // D - update DB
               _unitOfWork.Repository<Deploy>().Add(deploy);

               // E - update CVRef.Deploy fields
               var cvref = await _context.CVRefs.FindAsync(deploy.CVRefId);
               cvref.DeployStageId = deploy.StageId;
               cvref.DeployStageDate = deploy.TransactionDate;
               _unitOfWork.Repository<CVRef>().Update(cvref);

               // F - issue tasks
               var cvreviewid = await _context.CVRefs.Where(x => x.Id == deploy.CVRefId).Select(x => x.CVReviewId).FirstOrDefaultAsync();
               EnumTaskType thisTaskType=EnumTaskType.None;
               int AssignedToId=0;
               var commondata = await _commonServices.CommonDataFromCVRefId(deploy.CVRefId);
               switch((EnumDeployStatus)deploy.NextStageId)
               {
                    case EnumDeployStatus.Selected:
                         thisTaskType = EnumTaskType.OfferLetterAcceptance;
                         AssignedToId = commondata.HRExecId;
                         break;

                    case EnumDeployStatus.OfferLetterAccepted:
                         thisTaskType = EnumTaskType.MedicalTestsMobilization;
                         AssignedToId = commondata.MedicalProcessInchargeEmpId;
                         break;

                    case EnumDeployStatus.ReferredForMedical:
                         thisTaskType = EnumTaskType.MedicallyFit;
                         AssignedToId = commondata.VisaProcessInchargeEmpId;
                         break;

                    /* case EnumDeployStatus.MedicallyFit:
                         thisTaskType = EnumTaskType.VisaDocsKSACompilation;
                         AssignedToId = commondata.EmigProcessInchargeId;
                         break;

                    case EnumDeployStatus.MedicallyUnfit:
                         thisTaskType = EnumTaskType.None;
                         break;
                    */
                    case EnumDeployStatus.VisaDocsPrepared:
                         thisTaskType = EnumTaskType.VisaDocSubmission;
                         AssignedToId = commondata.VisaProcessInchargeEmpId;
                         break;
                    /*
                    case EnumDeployStatus.VisaDocsSubmitted:
                         thisTaskType = EnumTaskType.VisaReceived;
                         AssignedToId = commondata.EmigProcessInchargeId;
                         break;

                    case EnumDeployStatus.VisaQueryRaised:
                         thisTaskType = EnumTaskType.VisaReceived;
                         AssignedToId = commondata.EmigProcessInchargeId;
                         break;
                    */
                    case EnumDeployStatus.VisaReceived:
                         thisTaskType = EnumTaskType.EmigrationAppLodging;
                         AssignedToId = commondata.TravelProcessInchargeId;
                         break;
                    /*
                    case EnumDeployStatus.VisaDenied:
                         thisTaskType = EnumTaskType.None;
                         break;
                    */
                    case EnumDeployStatus.EmigDocsLodgedOnLine:
                         thisTaskType = EnumTaskType.EmigrationDocsSubmitted;
                         AssignedToId = commondata.TravelProcessInchargeId;
                         break;
                    /*
                    case EnumDeployStatus.EmigDocsPPSubmitted:
                         thisTaskType = EnumTaskType.EmigrationGranted;

                         break;

                    case EnumDeployStatus.EmigDocsQueryRecd:
                         thisTaskType = EnumTaskType.EmigrationDocsSubmitted;
                         break;

                    case EnumDeployStatus.EmigrationDenied:
                         thisTaskType = EnumTaskType.None;
                         break;
                    */
                    case EnumDeployStatus.EmigrationGranted:
                         thisTaskType = EnumTaskType.TravelTicketBooking;
                         break;
                    
                    case EnumDeployStatus.TravelTicketBooked:
                         thisTaskType = EnumTaskType.Traveled;
                         break;

                    case EnumDeployStatus.Traveled:
                         thisTaskType = EnumTaskType.ArrivalAcknowledgedByClient;
                         break;
                    /*
                    case EnumDeployStatus.TravelCanceled:
                         thisTaskType = EnumTaskType.None;
                         break;
                    */
                    case EnumDeployStatus.ArrivalAcknowledgedByClient:
                         thisTaskType = EnumTaskType.None;
                         break;
                    default:
                         break;
                    
               }
               
               if(AssignedToId !=0)
               {
                    var task = new ApplicationTask((int)thisTaskType, dt, loggedInEmployeeId, AssignedToId,
                         commondata.OrderId, commondata.OrderNo, commondata.OrderItemId, "Task for you to organize " +
                         ProcessName(deploy.NextStageId) + " for " + commondata.CandidateDesc, deploy.NextEstimatedStageDate,
                         "Open", commondata.CandidateId, cvreviewid);
                    _unitOfWork.Repository<ApplicationTask>().Add(task);
               }
               if (await _unitOfWork.Complete() > 0) return _mapper.Map<Deploy, DeployAddedDto>(deploy);

               return null;
          }

          public async Task<ICollection<DeployAddedDto>> AddDeploymentTransactions(ICollection<DeployPostDto> deployPosts, int loggedInEmployeeId)
          {
               // A - if transDate missing, make it current date
               // B - Create a model based on input parameters,
               // C - verify stageId sequence, and update NextSTageId and NextSTageEstiamted date values
               // D - update DB
               // E - update CVRef.DeployStage values
               // F - Issue tasks for next process

               DateTime dt;
               var deploys = new List<Deploy>();
               foreach(var post in deployPosts)
               {
                    dt = post.TransDate;

                    // B -
                         var deploy = new Deploy(post.CVRefId, dt, post.StageId);

                    // C - verify stageId sequence, and update NextSTageId and NextSTageEstiamted date values
                         deploy = await VerifyModelStageInSeqAndUpdateNextStage(deploy);       //also updates nexTStageId and dAte
                         if (deploy == null) return null;

                    // D - update DB
                         _unitOfWork.Repository<Deploy>().Add(deploy);
                         deploys.Add(deploy);

                    // E - update CVRef.Deploy fields
                         var cvref = await _context.CVRefs.FindAsync(deploy.CVRefId);
                         cvref.DeployStageId = deploy.StageId;
                         cvref.DeployStageDate = deploy.TransactionDate;
                         _unitOfWork.Repository<CVRef>().Update(cvref);

                    // F - issue tasks
                         var cvreviewid = await _context.CVRefs.Where(x => x.Id == deploy.CVRefId).Select(x => x.CVReviewId).FirstOrDefaultAsync();
                         EnumTaskType thisTaskType=EnumTaskType.None;
                         int AssignedToId=0;
                         var commondata = await _commonServices.CommonDataFromCVRefId(deploy.CVRefId);
                         switch(deploy.NextStageId)
                         {
                              case EnumDeployStatus.Selected:
                                   thisTaskType = EnumTaskType.OfferLetterAcceptance;
                                   AssignedToId = commondata.HRExecId;
                                   break;

                              case EnumDeployStatus.OfferLetterAccepted:
                                   thisTaskType = EnumTaskType.MedicalTestsMobilization;
                                   AssignedToId = commondata.MedicalProcessInchargeEmpId;
                                   break;

                              case EnumDeployStatus.ReferredForMedical:
                                   thisTaskType = EnumTaskType.MedicallyFit;
                                   AssignedToId = commondata.VisaProcessInchargeEmpId;
                                   break;

                              /* case EnumDeployStatus.MedicallyFit:
                                   thisTaskType = EnumTaskType.VisaDocsKSACompilation;
                                   AssignedToId = commondata.EmigProcessInchargeId;
                                   break;

                              case EnumDeployStatus.MedicallyUnfit:
                                   thisTaskType = EnumTaskType.None;
                                   break;
                              */
                              case EnumDeployStatus.VisaDocsPrepared:
                                   thisTaskType = EnumTaskType.VisaDocSubmission;
                                   AssignedToId = commondata.VisaProcessInchargeEmpId;
                                   break;
                              /*
                              case EnumDeployStatus.VisaDocsSubmitted:
                                   thisTaskType = EnumTaskType.VisaReceived;
                                   AssignedToId = commondata.EmigProcessInchargeId;
                                   break;

                              case EnumDeployStatus.VisaQueryRaised:
                                   thisTaskType = EnumTaskType.VisaReceived;
                                   AssignedToId = commondata.EmigProcessInchargeId;
                                   break;
                              */
                              case EnumDeployStatus.VisaReceived:
                                   thisTaskType = EnumTaskType.EmigrationAppLodging;
                                   AssignedToId = commondata.TravelProcessInchargeId;
                                   break;
                              /*
                              case EnumDeployStatus.VisaDenied:
                                   thisTaskType = EnumTaskType.None;
                                   break;
                              */
                              case EnumDeployStatus.EmigDocsLodgedOnLine:
                                   thisTaskType = EnumTaskType.EmigrationDocsSubmitted;
                                   AssignedToId = commondata.TravelProcessInchargeId;
                                   break;
                              /*
                              case EnumDeployStatus.EmigDocsPPSubmitted:
                                   thisTaskType = EnumTaskType.EmigrationGranted;

                                   break;

                              case EnumDeployStatus.EmigDocsQueryRecd:
                                   thisTaskType = EnumTaskType.EmigrationDocsSubmitted;
                                   break;

                              case EnumDeployStatus.EmigrationDenied:
                                   thisTaskType = EnumTaskType.None;
                                   break;
                              */
                              case EnumDeployStatus.EmigrationGranted:
                                   thisTaskType = EnumTaskType.TravelTicketBooking;
                                   break;
                              
                              case EnumDeployStatus.TravelTicketBooked:
                                   thisTaskType = EnumTaskType.Traveled;
                                   break;

                              case EnumDeployStatus.Traveled:
                                   thisTaskType = EnumTaskType.ArrivalAcknowledgedByClient;
                                   break;
                              /*
                              case EnumDeployStatus.TravelCanceled:
                                   thisTaskType = EnumTaskType.None;
                                   break;
                              */
                              case EnumDeployStatus.ArrivalAcknowledgedByClient:
                                   thisTaskType = EnumTaskType.None;
                                   break;
                              default:
                                   break;
                         }
                    
                         if(AssignedToId !=0)
                         {
                              var task = new ApplicationTask((int)thisTaskType, dt, loggedInEmployeeId, AssignedToId,
                                   commondata.OrderId, commondata.OrderNo, commondata.OrderItemId, "Task for you to organize " +
                                   ProcessName(deploy.NextStageId) + " for " + commondata.CandidateDesc, deploy.NextEstimatedStageDate,
                                   "Open", commondata.CandidateId, cvreviewid);
                              _unitOfWork.Repository<ApplicationTask>().Add(task);
                         }
               }
               // A - 
               if (await _unitOfWork.Complete() > 0) return _mapper.Map<ICollection<Deploy>, ICollection<DeployAddedDto>>(deploys);

               return null;
          }
         
          public async Task<bool> DeleteDeploymentTransactions(Deploy deploy)
          {
               var deployToDelete = await _context.Deploys.FindAsync(deploy.Id);
               var dep = await _context.Deploys
                    .Where(x => x.CVRefId == deploy.CVRefId && x.Id != deploy.Id)
                    .OrderByDescending(x => x.TransactionDate)
                    .Select(x => new { x.StageId, x.TransactionDate }).Take(1).FirstOrDefaultAsync();

               _unitOfWork.Repository<Deploy>().Delete(deployToDelete);

               await _unitOfWork.Complete();

               return await UpdateCVRefWithDeployLastRecord(deploy.CVRefId);
          }

         
          public async Task<bool> EditDeploymentTransaction(Deploy deploy)
          {
               _unitOfWork.Repository<Deploy>().Update(deploy);
               await _unitOfWork.Complete();

               return await UpdateCVRefWithDeployLastRecord(deploy.CVRefId);

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

          private async Task<Deploy> VerifyModelStageInSeqAndUpdateNextStage(Deploy deploy)
          {
               //return deploy model if in sequence, else return null;
               var lastStatusStageId = await _context.Deploys.Where(x => x.CVRefId == deploy.CVRefId)
               .MaxAsync(x => (int?)x.StageId) ?? 0;
               //var nextQuestionNo = await _context.AssessmentQsBank.MaxAsync(x => (int?)x.QNo) ?? 1;

               if ((EnumDeployStatus)lastStatusStageId == EnumDeployStatus.Concluded) return null;

               //  todo - return null if flg is out of sequence
               bool statusNotRelevantToProcess = false;
               EnumDeployStatus iStageId=EnumDeployStatus.None;
               EnumDeployStatus iNextStageId=EnumDeployStatus.None;
               switch (deploy.StageId)
               {
                    //following actions are not relevant to the process, hence can be out of sequence

                    case EnumDeployStatus.OfferLetterAccepted:
                         deploy.NextStageId = EnumDeployStatus.ReferredForMedical;
                         statusNotRelevantToProcess = true;
                         break;
                    case EnumDeployStatus.VisaQueryRaised:
                         deploy.NextStageId = EnumDeployStatus.VisaReceived;
                         statusNotRelevantToProcess = true;
                         break;
                    case EnumDeployStatus.EmigDocsQueryRecd:
                         deploy.NextStageId = EnumDeployStatus.EmigrationGranted;
                         statusNotRelevantToProcess = true;
                         break;
                    default:
                         break;
               }
               if (statusNotRelevantToProcess) return deploy;

               // C 
               //status is relevant to process
               //if status id is blank, offer next logical stages
               var dep = await _context.Deploys.Where(x => x.CVRefId == deploy.CVRefId)
                   .OrderByDescending(x => x.StageId).Take(1).FirstOrDefaultAsync();           //latest record
               if (dep == null)              //no record, so likely it is first entry after selection
               {
                    if (deploy.StageId != EnumDeployStatus.Selected)
                    {
                         return null;
                    }
               }
               else
               {
                    //if model does not have a stageId defined, then define one
                    iStageId = deploy.StageId == EnumDeployStatus.None 
                         ? await _context.Deploys.Where(x => x.CVRefId == dep.CVRefId)
                              .OrderByDescending(x => x.StageId).Select(x => x.StageId).FirstOrDefaultAsync()
                         : deploy.StageId;
                    deploy.StageId = iStageId;

                    var nextStage = await _context.DeployStatus.Where(x => (EnumDeployStatus)x.StageId == iStageId)
                         .Select(x => new { x.NextStageId, x.WorkingDaysReqdForNextStage }).FirstOrDefaultAsync();

                    if (nextStage == null) return null;
                    
                    if (iNextStageId != EnumDeployStatus.Concluded)
                    {
                         var isCandidateEcnr = false;
                         //TODO - add working days instead of days
                         deploy.StageId = iStageId;
                         if (deploy.StageId == EnumDeployStatus.VisaReceived)
                         {
                              isCandidateEcnr = await _context.CVRefs.Where(x => x.Id == dep.CVRefId)
                                   .Select(x => x.Ecnr).FirstOrDefaultAsync();
                         }
                         deploy.NextStageId = (isCandidateEcnr && iStageId == EnumDeployStatus.VisaReceived)
                              ? EnumDeployStatus.TravelTicketBooked : (EnumDeployStatus)nextStage.NextStageId;

                         deploy.NextEstimatedStageDate = deploy.TransactionDate.AddDays(nextStage.WorkingDaysReqdForNextStage);
                    }
               }
               return deploy;
          }
          private async Task<string> ProcessName(EnumDeployStatus ProcessId)
          {
               return await _context.DeployStatus.Where(x => (EnumDeployStatus)x.Id == ProcessId).Select(x => x.ProcessName).FirstOrDefaultAsync();
          }

          private async Task<bool> UpdateCVRefWithDeployLastRecord(int cvrefid)
          {
               var deployLastRecord = await _context.Deploys.Where(x => x.CVRefId == cvrefid)
               .OrderByDescending(x => x.TransactionDate).Take(1).FirstOrDefaultAsync();

               var cvrefToUpdate = await _context.CVRefs.FindAsync(cvrefid);
               cvrefToUpdate.DeployStageId = deployLastRecord.StageId;
               cvrefToUpdate.DeployStageDate = deployLastRecord.TransactionDate;

               _unitOfWork.Repository<CVRef>().Update(cvrefToUpdate);
               return await _unitOfWork.Complete() > 0;
          }
     }
}