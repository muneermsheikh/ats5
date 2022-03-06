using System;
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
     //[Authorize]
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
          
          //[Authorize]         //(Policy = "OrdersViewReportRole")]
          [HttpGet("ordersbriefpaginated")]
          public async Task<ActionResult<Pagination<OrderToReturnDto>>> GetOrdersBriefAll([FromQuery]OrdersSpecParams orderParams)
          {
               //               var orders = await _orderService.GetOrdersBriefAllAsync(orderParams);
               var orders = await _orderService.GetOrdersAllAsync(orderParams);
               if (orders == null) return NotFound(new ApiResponse(400, "No orders found"));

               return Ok(orders);
          }

          [HttpGet("openorderitemsnotpaged")]
          public async Task<ICollection<OrderItemBriefDto>> GetCurrentOpeningsDto()
          {
               return await _orderService.GetOpenOrderItemsNotPaged();
          }
          [HttpGet("byid/{id}")]
          public async Task<ActionResult<Order>> GetOrderById (int id)
          {
               var order = await _orderService.GetOrderByIdAsyc(id);

               if (order !=null) return Ok(order);

               return NotFound();
          }

          [HttpGet("ordercities")]
          public async Task<ICollection<CustomerCity>> GetOrderCities ()
          {
               return (ICollection<CustomerCity>)await _orderService.GetOrderCityNames();
          }

          //[Authorize(Policy = "OrdersCreateRole")] 
          [HttpPost]
          public async Task<ActionResult<Order>> CreateOrder(OrderToCreateDto dto)
          {
               var loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
               dto.LoggedInAppUserId = loggedInUser.Id;
               var order = await _orderService.CreateOrderAsync(dto);
               if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

               return Ok(order);
          }

          [HttpPost("orders")]
          public async Task<ActionResult<ICollection<Order>>> CreateOrders(ICollection<OrderToCreateDto> dtos)
          {
               var loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);

               var order = await _orderService.CreateOrdersAsync(loggedInUser.Id, dtos);

               if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

               return Ok(order);
          }

          //[Authorize(Policy = "OrdersCreateRole")]
          [HttpPut]
          public async Task<ActionResult<bool>> EditOrder(Order order)
          {
               /* if (User == null) return Unauthorized("User required to log in");
               var loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
               if (loggedInUser == null) return Unauthorized("authorized user required to log in");
               */
               if ( await _orderService.EditOrder(order, 0)) return Ok(true);
               return BadRequest(new ApiResponse(400, "Problem updating the order"));
          }
          
          [HttpPut("updatedlfwd")]
          public async Task<bool> UpdateOrderDLForwardedToHR(IdAndDate idanddate)
          {
               return await _orderService.UpdateDLForwardedDateToHR(idanddate.OrderId, idanddate.DateForwarded);
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

          [HttpGet("jd/{orderitemid}")]
          public async Task<ActionResult<JDDto>> GetOrCreateJD(int orderitemid)
          {
               var jd = await _orderService.GetOrAddJobDescription(orderitemid);

               if (jd == null) return BadRequest(new ApiResponse(404, "Failed to get or create job description"));

               return Ok(jd);

          }

          [HttpGet("remuneration/{orderitemid}")]
          public async Task<ActionResult<RemunerationFullDto>> GetOrCreateRemuneration(int orderitemid)
          {
               var remun = await _orderService.GetOrAddRemunerationByOrderItemAsync(orderitemid);

               if (remun == null) return BadRequest(new ApiResponse(404, "Failed to get or create the remuneration details"));

               return Ok(remun);

          }


          [HttpPut("jd")]
          public async Task<ActionResult<bool>> UpdateJD (JDDto jddto)
          {
               return await _orderService.EditJobDescription(jddto);
          }

          
          [HttpPut("remuneration")]
          public async Task<ActionResult<bool>> UpdateRemuneration (RemunerationFullDto dto)
          {
               return await _orderService.EditRemuneration(dto);
          }
          
     }
}