using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Errors;
using api.Extensions;
using api.Helpers;
using AutoMapper;
using core.Entities.Identity;
using core.Entities.Orders;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     [Authorize]
     public class OrdersController : BaseApiController
     {
          private readonly IOrderService _orderService;
          private readonly IMapper _mapper;
          private readonly IGenericRepository<Order> _orderRepo;
          private readonly UserManager<AppUser> _userManager;
          public OrdersController(IOrderService orderService, IMapper mapper, IGenericRepository<Order> orderRepo, UserManager<AppUser> userManager)
          {
               _userManager = userManager;
               _orderRepo = orderRepo;
               _mapper = mapper;
               _orderService = orderService;
          }


          [Authorize]         //(Policy = "OrdersViewReportRole")]
          [HttpGet]
          public async Task<ActionResult<Pagination<OrderToReturnDto>>> GetOrdersAll(OrdersSpecParams orderParams)
          {
               var orders = await _orderService.GetOrdersAllAsync(orderParams);
               if (orders == null) return NotFound(new ApiResponse(400, "No orders found"));

               return Ok(orders);
          }

          [Authorize(Policy = "OrdersCreateRole")]
          [HttpPost]
          public async Task<ActionResult<Order>> CreateOrder(OrderToCreateDto dto)
          {
               var loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
               dto.LoggedInAppUserId = loggedInUser.Id;
               var order = await _orderService.CreateOrderAsync(dto);
               if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

               return Ok(order);
          }

          [Authorize(Policy = "OrdersCreateRole")]
          [HttpPut]
          public async Task<ActionResult<bool>> EditOrder(Order order)
          {
               var loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
               if ( await _orderService.EditOrder(order)) return Ok(true);
               return BadRequest(new ApiResponse(400, "Problem updating the order"));
          }
          
          //remunerations
          [Authorize]         //(Policy = "OrdersCreateRole")]
          [HttpPost("remun")]
          public async Task<ActionResult<Remuneration>> CreateRemuneration(Remuneration remuneration)
          {
               var remun = await _orderService.AddRemuneration(remuneration);
               if (remun == null) return BadRequest(new ApiResponse(400, "failed to save the remuneration detail"));

               //return Ok(_mapper.Map<Remuneration, RemunerationDto>(remun));
               return Ok(remun);
          }
     }
}