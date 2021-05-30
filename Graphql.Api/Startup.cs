using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gateway.Graphql;
using HotChocolate.AspNetCore;

namespace Graphql.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) 
            => services
                .InitHttpClients(Configuration)
                .RegistergGraphqlTypes();

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app.UseDeveloperExceptionPage()
                .UseRouting()
                .UseEndpoints(endpoints => endpoints.MapGraphQL())
                .UsePlayground();
    }
}
