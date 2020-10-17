using Bank.Application.Accounts;
using Bank.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Application
{
    public static class Config
    {
        public static void AddBankAccountManagement(this IServiceCollection services, IConfiguration config)
        {
            services.AddMarten(config, options => Accounts.Config.ConfigureMarten(options));
            services.AddBankAccount();
        }
    }
}
