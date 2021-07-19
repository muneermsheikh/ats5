using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Errors;
using api.Helpers;
using AutoMapper;
using core.Entities.Orders;
using core.Interfaces;
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
          public async Task<ActionResult<Pagination<OrderToReturnDto>>> GetOrdersAll(OrderParams orderParams)
          {
               var spec = new OrdersWithItemsAndOrderingSpecs();
               var countSpec = new OrdersWithItemsAndOrderingForCountSpecs();

               var totalItems = await _orderRepo.CountAsync(spec);
               var orders = await _orderRepo.ListAsync(spec);

               var data = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);

               return Ok(new Pagination<OrderToReturnDto>(orderParams.PageIndex,
                    orderParams.PageSize, totalItems, data));
          }

          [HttpGet("{id}")]
          public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int id)
          {
               var order = await _orderService.GetOrderByIdAsync(id);
               if (order == null) return NotFound(new ApiResponse(404));
               return _mapper.Map<OrderToReturnDto>(order);
          }

          [HttpPost]
          public async Task<ActionResult<Order>> CreateOrder(OrderToCreateDto dto)
          {
               var order = await _orderService.CreateOrderAsync(dto);
               if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

               return Ok(order);
          }
     }
}