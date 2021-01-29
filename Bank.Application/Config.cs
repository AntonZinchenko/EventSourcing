using Bank.Application.Accounts;
using Bank.Application.Processing;
using Bank.Storage;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Application
{
    public static class Config
    {
        public static void AddBankAccountManagement(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddMarten(config, options => Accounts.Config.ConfigureMarten(options));
            services.AddBankAccount();
        }
    }
}
