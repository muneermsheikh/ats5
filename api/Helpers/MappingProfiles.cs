using System;
using api.DTOs;
using AutoMapper;
using core.Entities;
using core.Entities.HR;
using core.Entities.Orders;
using core.Entities.Users;
using core.Entities.Identity;
using core.ParamsAndDtos;

namespace api.Helpers
{
     public class MappingProfiles : Profile
     {
          public MappingProfiles()
          {
               CreateMap<Customer, CustomerTypeNameKnownAsOfficialsToReturnDto>();
               CreateMap<Customer, CustomerDto>();
               CreateMap<CVRef, CVRefDto>();
               CreateMap<OrderItem, OrderItemDto>();
               CreateMap<Order, OrderToReturnDto>();
               CreateMap<RegisterDto, Candidate>()
                    .ForMember(d => d.FirstName, o => o.MapFrom(s => s.Address.FirstName))
                    .ForMember(d => d.SecondName, o => o.MapFrom(s => s.Address.SecondName))
                    .ForMember(d => d.FamilyName, o => o.MapFrom(s => s.Address.FamilyName))
                    .ForMember(d => d.KnownAs, o => o.MapFrom(s => s.DisplayName))
                    .ForMember(d => d.DOB, o => o.MapFrom(s => s.Address.DOB))
                    ;
               CreateMap<Address, EntityAddress>();
               
          }
     }
}