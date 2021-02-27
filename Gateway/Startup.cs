using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.AspNetCore;
using Gateway.Graphql;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Gateway
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
            services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

           services.AddOcelot(Configuration);

            services.InitHttpClients(Configuration)
                .RegistergGraphqlTypes();
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();

            app.UseRouting()
                .UseWebSockets()
                .UseGraphQL()
                .UsePlayground();

            await app.UseOcelot();
        }
    }
}
