using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Errors;
using api.Helpers;
using AutoMapper;
using core.Entities.Orders;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     [Authorize]
     public class OrdersController : BaseApiController
     {
          private readonly IOrderService _orderService;
          private readonly IMapper _mapper;
          private readonly IGenericRepository<Order> _orderRepo;
          public OrdersController(IOrderService orderService, IMapper mapper, IGenericRepository<Order> orderRepo)
          {
               _orderRepo = orderRepo;
               _mapper = mapper;
               _orderService = orderService;
          }

          
          [HttpGet]
          public async Task<ActionResult<Pagination<OrderToReturnDto>>> GetOrdersAll(OrdersSpecParams orderParams)
          {
               var orders = await _orderService.GetOrdersAllAsync(orderParams);
               if (orders == null ) return NotFound (new ApiResponse(400, "No orders found"));
               
               return Ok(orders);
          }

          [HttpPost]
          public async Task<ActionResult<Order>> CreateOrder(OrderToCreateDto dto)
          {
               var order = await _orderService.CreateOrderAsync(dto);
               if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

               return Ok(order);
          }

     //remunerations
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