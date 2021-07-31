using System.Threading.Tasks;
using api.Errors;
using core.Entities.Admin;
using core.Entities.Identity;
using core.Interfaces;
using core.Params;
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
               var emps = await _empService.GetEmployeePaginated(empParams);
               if (emps == null) return NotFound(new ApiResponse(404, "No employees found"));
               return Ok(emps);
          }

          [HttpPut]
          public async Task<ActionResult<bool>> EditEmployee(Employee employee)
          {
               var email = employee.Email;
               if (string.IsNullOrEmpty(email)) return BadRequest(new 
                    ApiResponse(400, "email Id for employee " + employee.FirstName + " " + employee.SecondName + " " + employee.FamilyName + 
                    " not provided"));
               var user = (await _userManager.FindByIdAsync(employee.AppUserId) == null);
               if (user == false) {
                    return BadRequest(new ApiResponse(404, "Bad Request - this employee identity is not registered - go for employee add and not edit"));
               }
               
               if (await _userManager.FindByEmailAsync(email) != null) {
                    return !await _empService.EditEmployee(employee);
               }

               return BadRequest(new ApiResponse(400, "failed to update the employee"));
          }

          [HttpDelete]
          public async Task<ActionResult<bool>> DeleteEmployee(Employee employee)
          {
               await _empService.DeleteEmployee(employee);

               if (await _unitOfWork.Complete() > 0)
               {
                    var user = await _userManager.FindByIdAsync(employee.AppUserId);
                    if (user != null) await _userManager.DeleteAsync(user);
                    return Ok();
               }

               return BadRequest(new ApiResponse(400, "Failed to delete the employee"));
          }
     
          [HttpPost]
          public async Task<ActionResult<Employee>> AddNewEmployee(Employee employee)
          {
               var email = employee.Email;
               if (string.IsNullOrEmpty(email)) return BadRequest(new 
                    ApiResponse(400, "email Id for employee " + employee.FirstName + " " + employee.SecondName + " " + employee.FamilyName + 
                    " not provided"));
               if (await _userManager.FindByEmailAsync(email) != null)
               {
                    return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Email address " + email + " is in use" } });
               }

               if(string.IsNullOrEmpty(employee.Password)) {
                    return BadRequest(new ApiResponse(400, "Password essential"));
               }
          
               return await _empService.AddNewEmployee(employee);

          }
     }
}