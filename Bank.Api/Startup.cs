using Bank.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SeedWorks;
using System;
using System.IO;
using System.Linq;
using Hellang.Middleware.ProblemDetails;
using Bank.Api.Validation;
using SeedWorks.Validation;
using Bank.Storage;
using Microsoft.AspNetCore.Http;
using Bank.Orchestrators;

namespace Bank.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Event source sandbox", Version = "v1" });

                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(file => c.IncludeXmlComments(file));
            });

            services.AddProblemDetails(x =>
            {
                x.Map<FluentValidation.ValidationException>(ex => new FluentValidationExceptionProblemDetails(ex));
                x.Map<EntityNotFoundException>(ex => new EntityNotExistsDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();

            services.AddCoreServices();
            services.AddBankAccountManagement(Configuration);
            services.ConfigOrchestrators(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "Event source sandbox V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseProblemDetails();
            app.UseMiddleware<CorrelationMiddleware>();

            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
