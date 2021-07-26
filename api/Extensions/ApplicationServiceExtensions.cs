using System.Linq;
using api.Errors;
using core.Interfaces;
using infra.Data;
using infra.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace api.Extensions
{
    public static class ApplicationServiceExtensions
    {
            public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            {
                //services.AddSingleton<IResponseCacheService, ResponseCacheService>();
                
                services.AddScoped<ITokenService, TokenService>();
                services.AddScoped<ICategoryRepository, CategoryRepository>();
                //services.AddScoped<IMessageRepository, MessageRepository>();
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped<IOrderService, OrderService>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<ICustomerService, CustomerService>();
                services.AddScoped<IMastersService, MastersService>();
                services.AddScoped<IEmployeeService, EmployeeService>();
                services.AddScoped<IOrderAssessmentService, OrderAssessmentService>();
                services.AddScoped<ICandidateAssessmentService, CandidateAssessmentService>();
                services.AddScoped<ICVRefService, CVRefService>();
                services.AddScoped<IContractReviewService, ContractReviewService>();
                /*
                services.AddScoped<IPaymentService, PaymentService>();
                
                services.AddScoped<IBasketRepository, BasketRepository>();
                */
                services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            
                services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var errors = actionContext.ModelState
                            .Where(e => e.Value.Errors.Count > 0)
                            .SelectMany(x => x.Value.Errors)
                            .Select(x => x.ErrorMessage).ToArray();

                        var errorResponse = new ApiValidationErrorResponse
                        {
                            Errors = errors
                        };

                        return new BadRequestObjectResult(errorResponse);
                    };
                });

                return services;
            }
    }
}