using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Errors;
using api.Helpers;
using AutoMapper;
using core.Entities;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class CustomersController : BaseApiController
     {
          private readonly IGenericRepository<Customer> _custRepo;
          private readonly IMapper _mapper;
          public CustomersController(IGenericRepository<Customer> custRepo, IMapper mapper)
          {
               _mapper = mapper;
               _custRepo = custRepo;
          }

          [HttpGet]
          [ProducesResponseType(StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          public async Task<ActionResult<Pagination<CustomerIdKnownAsCityToReturnDto>>> GetCustomers(
               [FromQuery] CustomerSpecParams custParams)
          {
               var spec = new CustomerWithOfficialsSpecs(custParams);
               var countSpec = new CustomersWithFiltersForCountSpecs(custParams);
               var totalCount = await _custRepo.CountAsync(spec);
               if (totalCount == 0) return NotFound(new ApiResponse(404));
               
               var custs = await _custRepo.ListAsync(spec);
               var data = _mapper.Map<IReadOnlyList<Customer>, IReadOnlyList<CustomerIdKnownAsCityToReturnDto>>(custs);

               return Ok(new Pagination<CustomerIdKnownAsCityToReturnDto>(custParams.PageIndex, custParams.PageSize, totalCount, data));
          }

          [HttpGet("custbyid/{id}")]
          [ProducesResponseType(StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          public async Task<ActionResult<CustomerIdKnownAsCityToReturnDto>> GetCustomerById(int id)
          {
               var spec = new CustomerWithOfficialsSpecs(id);
               var cust = await _custRepo.GetEntityWithSpec(spec);
               if (cust == null) return NotFound(new ApiResponse(404));
               return Ok(_mapper.Map<Customer, CustomerIdKnownAsCityToReturnDto>(cust));
          }

     }
}