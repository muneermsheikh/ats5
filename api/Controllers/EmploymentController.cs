using System.Threading.Tasks;
using core.Entities.HR;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class EmploymentController : BaseApiController
     {
          private readonly IUnitOfWork _unitOfWork;
          public EmploymentController(IUnitOfWork unitOfWork)
          {
               _unitOfWork = unitOfWork;
          }

          [HttpGet]
          public async Task<ActionResult<Pagination<Employment>>> GetEmployments(EmploymentParams employmentParams)
          {
               var spec = new EmploymentSpecs(employmentParams);
               var specCount = new EmploymentForCountSpecs(employmentParams);
               var emps = await _unitOfWork.Repository<Employment>().ListAsync(spec);
               var ct = await _unitOfWork.Repository<Employment>().CountAsync(specCount);

               return Ok(new Pagination<Employment>(employmentParams.PageIndex,
                    employmentParams.PageSize, ct, emps));
          }

          [HttpPost]
          public async Task<ActionResult<Employment>> AddEmployment(Employment employment)
          {
              // todo - verify object
              var sel = await _unitOfWork.Repository<SelectionDecision>().GetByIdAsync(employment.SelectionDecisionId);
              if (sel == null) return null;

              _unitOfWork.Repository<Employment>().Add(employment);

              if (await _unitOfWork.Complete() > 0) {
                  return await _unitOfWork.Repository<Employment>().GetEntityWithSpec(
                      new EmploymentSpecs(new EmploymentParams{CVRefId=employment.CVRefId})
                  );
              } else {
                  return null;
              }
          }

     }
}