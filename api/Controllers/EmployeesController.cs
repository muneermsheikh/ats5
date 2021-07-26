using System.Threading.Tasks;
using api.Errors;
using core.Entities.Admin;
using core.Entities.Identity;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class EmployeesController : BaseApiController
     {
          private readonly IGenericRepository<Employee> _empRepo;
          private readonly IEmployeeService _empService;
          private readonly IUnitOfWork _unitOfWork;
          private readonly UserManager<AppUser> _userManager;
          private readonly ATSContext _context;
          public EmployeesController(IGenericRepository<Employee> empRepo, IEmployeeService empService,
               IUnitOfWork unitOfWork, UserManager<AppUser> userManager, ATSContext context)
          {
               _context = context;
               _userManager = userManager;
               _unitOfWork = unitOfWork;
               _empService = empService;
               _empRepo = empRepo;
          }

     [HttpGet]
     public async Task<ActionResult<Pagination<Employee>>> GetEmployees(EmployeeSpecParams empParams)
     {
          var spec = new EmployeeSpecs(empParams);
          var countSpec = new EmployeeForCountSpecs(empParams);

          var totalItems = await _empRepo.CountAsync(countSpec);
          var emps = await _empRepo.ListAsync(spec);

          //var data = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);

          return Ok(new Pagination<Employee>(empParams.PageIndex,
               empParams.PageSize, totalItems, emps));
     }

     [HttpPut("editEmployee")]
     public async Task<ActionResult<bool>> EditEmployee(Employee employee)
     {
          _empService.EditEmployee(employee);

          if (await _unitOfWork.Complete() > 0)
          {
               //ToDo - edit identityuser details too    
              /* foreach (var entry in _context.Employees(employee).Properties)
               {
                    Console.WriteLine(
                        $"Property '{entry.Metadata.Name}'" +
                        $" is {(entry.IsModified ? "modified" : "not modified")} " +
                        $"Current value: '{entry.CurrentValue}' " +
                        $"Original value: '{entry.OriginalValue}'");
               }
               */
               return Ok();
          }
          return BadRequest(new ApiResponse(400, "failed to update the employee"));
     }

     [HttpDelete]
     public async Task<ActionResult<bool>> DeleteEmployee(Employee employee)
     {
          _empService.DeleteEmployee(employee);

          if (await _unitOfWork.Complete() > 0)
          {
               var user = await _userManager.FindByIdAsync(employee.AppUserId);
               if (user != null) await _userManager.DeleteAsync(user);
               return Ok();
          }

          return BadRequest(new ApiResponse(400, "Failed to delete the employee"));
     }
}
}