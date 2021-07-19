using api.Extensions;
using api.Helpers;
using infra.Data;
using infra.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace api
{
     public class Startup
     {
          private readonly IConfiguration _config;
          public Startup(IConfiguration config)
          {
               _config = config;
          }

          //public IConfiguration Configuration { get; }

          // This method gets called by the runtime. Use this method to add services to the container.
          public void ConfigureServices(IServiceCollection services)
          {

               services.AddAutoMapper(typeof(MappingProfiles));
               services.AddControllers();
               services.AddApplicationServices();
               services.AddIdentityServices(_config);
               
               services.AddDbContext<ATSContext>(x => x.UseSqlite(
                   _config.GetConnectionString("DefaultConnection")
               ));
               
               services.AddDbContext<AppIdentityDbContext>( x => 
               {
                    x.UseSqlite(_config.GetConnectionString("IdentityConnection"));
               });

               services.AddSwaggerGen(c =>
               {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
               });
               
               //services.AddSwaggerDocumentation();

               services.AddCors(opt =>
               {
                    opt.AddPolicy("CorsPolicy", policy =>
                    {
                         policy.AllowAnyHeader().AllowAnyMethod()
                         .WithOrigins("https://localhost:4200");
                    });
               });
          }

          // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
          //order in this procedure is imp
          public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
          {
               if (env.IsDevelopment())
               {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
               }

               app.UseHttpsRedirection();

               app.UseRouting();

               app.UseCors("CorsPolicy");

               app.UseAuthentication();
               app.UseAuthorization();

               app.UseSwagger();
               app.UseSwaggerUI(c =>
               {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ATS API V1");
               });

               app.UseEndpoints(endpoints =>
               {
                    endpoints.MapControllers();
               });
          }
     }
}
