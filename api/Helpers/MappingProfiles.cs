using System;
using api.DTOs;
using AutoMapper;
using core.Entities;
using core.Entities.HR;
using core.Entities.Orders;
using core.Entities.Users;
using core.Entities.Identity;
using core.ParamsAndDtos;
using core.Entities.EmailandSMS;
using core.Entities.Tasks;
using core.Entities.Process;

namespace api.Helpers
{
     public class MappingProfiles : Profile
     {
          public MappingProfiles()
          {
               CreateMap<ApplicationTask, TaskDashboardDto>();
               CreateMap<Customer, CustomerTypeNameKnownAsOfficialsToReturnDto>();
               CreateMap<Customer, CustomerDto>();
               CreateMap<CVRef, CVRefDto>();
               CreateMap<CVRef, CVRefAndDeployDto>();
                    //.ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
               CreateMap<CVRef, CVRefPostedDto>();
               CreateMap<CVRef, SelectionsPendingDto>();
               CreateMap<EmailMessage, MessageDto>();
               CreateMap<Employment, EmploymentToReturnDto>();
               CreateMap<EmployeeToAddDto, EmployeeDto>();
                    
               CreateMap<OrderItem, OrderItemDto>();
               CreateMap<OrderItem, OrderItemToReturnDto>();
               CreateMap<Order, OrderToReturnDto>();
               CreateMap<Remuneration, RemunerationDto>();
               CreateMap<SelectionDecision, SelectionDecisionToReturnDto>();
               
               CreateMap<RegisterDto, Candidate>()
                    .ForMember(d => d.FirstName, o => o.MapFrom(s => s.Address.FirstName))
                    .ForMember(d => d.SecondName, o => o.MapFrom(s => s.Address.SecondName))
                    .ForMember(d => d.FamilyName, o => o.MapFrom(s => s.Address.FamilyName))
                    .ForMember(d => d.KnownAs, o => o.MapFrom(s => s.DisplayName))
                    .ForMember(d => d.DOB, o => o.MapFrom(s => s.Address.DOB))
                    ;
               
               CreateMap<Candidate, CandidateInBriefDto>();
               
               CreateMap<Address, EntityAddress>();
               CreateMap<SelectionDecision, SelectionDecisionToRegisterDto>();

               CreateMap<ContractReview, ContractReviewDto>();
                    //.ForMember(d => d.ReviewStatus, o => o.MapFrom(s => s.ReviewStatus.Status));
               CreateMap<ContractReviewItem, ContractReviewItemDto>();

               CreateMap<SelDecisionToAddDto, RejDecisionToAddDto>();
               CreateMap<CVRef, DeploymentPendingDto>()
                    .ForMember(d => d.CVRefId, o => o.MapFrom(s => s.Id));
                    //.ForMember(d => d.DeployStageId, o => o.MapFrom(s => EnumDeployStatus.GetAttribute<s.DeployStageId>()));
               CreateMap<Deploy, DeployAddedDto>();

               CreateMap<UserProfession, Prof>();

               //itnerviews

          }
     }
}