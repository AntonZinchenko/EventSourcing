using BankAccount.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeedWorks;
using Hellang.Middleware.ProblemDetails;
using BankAccount.Api.Validation;
using SeedWorks.Validation;
using BankAccount.Storage;

namespace BankAccount.Api
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

            services.AddProblemDetails(x =>
            {
                x.Map<FluentValidation.ValidationException>(ex => new FluentValidationExceptionProblemDetails(ex));
                x.Map<EntityNotFoundException>(ex => new EntityNotExistsDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });

            services
                .AddCustomSwagger()
                .AddOptions(Configuration)
                .AddContextAccessor()
                .ConfigRabbitBus(Configuration)
                .ConfigApplication(Configuration);
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
                c.SwaggerEndpoint("./v1/swagger.json", "Bank accounts");
                c.RoutePrefix = "swagger";
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
