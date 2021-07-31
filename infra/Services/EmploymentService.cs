using System.Threading.Tasks;
using core.Entities.HR;
using core.Interfaces;
using core.Params;
using core.Specifications;

namespace infra.Services
{
     public class EmploymentService : IEmploymentService
     {
          private readonly IUnitOfWork _unitOfWork;
          public EmploymentService(IUnitOfWork unitOfWork)
          {
               _unitOfWork = unitOfWork;
          }

          public async Task<Employment> AddEmployment(Employment employment)
          {
               _unitOfWork.Repository<Employment>().Add(employment);
               if (await _unitOfWork.Complete() > 0) {
                   var empParams = new EmploymentParams{CVRefId = employment.CVRefId};
                   var specs = new EmploymentSpecs(empParams);

                   return await _unitOfWork.Repository<Employment>().GetEntityWithSpec(specs);
               } else {
                   return null;
               }
          }

          public async Task<bool> DeleteEmployment(Employment employment)
          {
               _unitOfWork.Repository<Employment>().Delete(employment);
               return await _unitOfWork.Complete() > 0;
          }

          public async Task<bool> EditEmployment(Employment employment)
          {
               // todo - verify object
                _unitOfWork.Repository<Employment>().Update(employment);

               return await _unitOfWork.Complete() > 0;
          }
     }
}