using System;
using api.DTOs;
using AutoMapper;
using core.Entities;
using core.Entities.HR;
using core.Entities.Orders;
using core.ParamsAndDtos;

namespace api.Helpers
{
     public class MappingProfiles : Profile
     {
          public MappingProfiles()
          {
               CreateMap<Customer, CustomerTypeNameKnownAsOfficialsToReturnDto>();
               CreateMap<CVRef, CVRefDto>();
               CreateMap<OrderItem, OrderItemDto>();
               CreateMap<Order, OrderToReturnDto>();
               CreateMap<RegisterDto, CandidateToCreateDto>();
          }
     }
}