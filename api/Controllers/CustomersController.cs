using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Errors;
using api.Helpers;
using AutoMapper;
using core.Entities;
using core.Entities.Identity;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize(Policy = "Employee, CustomerMaintenanceRole")]
     public class CustomersController : BaseApiController
     {
          private readonly IGenericRepository<Customer> _custRepo;
          private readonly IMapper _mapper;
          private readonly IUnitOfWork _unitOfWork;
          private readonly ICustomerService _customerService;
          private readonly UserManager<AppUser> _usermanager;
          public CustomersController(IGenericRepository<Customer> custRepo, IMapper mapper,
            IUnitOfWork unitOfWork, ICustomerService customerService, UserManager<AppUser> usermanager)
          {
               _usermanager = usermanager;
               _customerService = customerService;
               _unitOfWork = unitOfWork;
               _mapper = mapper;
               _custRepo = custRepo;
          }

        [HttpPost("registercustomer")]
        public async Task<ActionResult<CustomerDto>> RegisterCustomer(RegisterCustomerDto dto)
        {
            foreach (var em in dto.CustomerOfficials)
            {
                var email = em.Email;
                if (string.IsNullOrEmpty(email)) return BadRequest(new 
                        ApiResponse(400, "email Id for official " + em.FirstName + " " + em.SecondName + " " + em.FamilyName + 
                        " not provided"));
                if (CheckEmailExistsAsync(email).Result.Value)
                {
                        return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Email address " + email + " is in use" } });
                }

                if(em.LogInCredential && string.IsNullOrEmpty(em.Password)) {
                    return BadRequest(new ApiResponse(400, "Password for logInCredential users essential"));
                }
            }

            return await _customerService.AddCustomer(dto);
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<Customer>>> GetCustomers(CustomerSpecParams custParams)
        {
            var specs = new CustomerWithOfficialsSpecs(custParams);
            var countSpec = new CustomersWithFiltersForCountSpecs(custParams);
            var customers = await _unitOfWork.Repository<Customer>().ListAsync(specs);
            var totalCount = await _unitOfWork.Repository<Customer>().CountAsync(countSpec);

            return Ok(new Pagination<Customer>(custParams.PageIndex, custParams.PageSize, totalCount, customers));

        }

        [HttpGet("associateidandnames/{usertype}")]
        public async Task<ActionResult<ICollection<CustomerIdAndNameDto>>> GetCustomerIdAndNames(string usertype)
        {
            return Ok(await _customerService.GetCustomerIdAndName(usertype));
        }

        [HttpGet("byId/{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int Id)
        {
            var cust = await _unitOfWork.Repository<Customer>().GetByIdAsync(Id);
            return Ok(_mapper.Map<Customer, CustomerDto>(cust));
        }


        [HttpPut]
        public async Task<ActionResult> UpdateCustomer(Customer customer)
        {
            _customerService.EditCustomer(customer);

            if (await _unitOfWork.Complete() > 0) return NoContent();

            return BadRequest();
        }

        //officialid and names
        [HttpGet("officialidandname/{custType}")]
        public async Task<ActionResult<ICollection<CustomerOfficialIdAndNameDto>>> CustomerOfficialIdAndNames(string custType)
        {
            var users = await _customerService.GetCustomerIdAndName(custType);
            return Ok(users);
        }

        private async Task<ActionResult<bool>> CheckEmailExistsAsync(string email)
        {
            return await _usermanager.FindByEmailAsync(email) != null;
        }
    }
}