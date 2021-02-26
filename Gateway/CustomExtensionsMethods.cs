using Graphql.Graphql.HttpClients;
using Graphql.Graphql.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeedWorks.HttpClients;
using System;

namespace Gateway
{
    public static class CustomExtensionsMethods
    {
        /// <summary>
        /// Инициализация http клиентов к вспомогательным сервисам.
        /// </summary>
        public static IServiceCollection InitHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<LoggingHttpClientHandler>();
            var buidler = services.BuildServiceProvider();

            services.AddHttpClient<IBankAccountClient, BankAccountClient>(
                c =>
                {
                    c.BaseAddress = new Uri(configuration["BankAccountHost"]);
                    c.Timeout = TimeSpan.FromMinutes(5);
                }).ConfigurePrimaryHttpMessageHandler(c => buidler.GetService<LoggingHttpClientHandler>());

            services.AddHttpClient<ITransferClient, TransferClient>(
                c =>
                {
                    c.BaseAddress = new Uri(configuration["TransferHost"]);
                    c.Timeout = TimeSpan.FromMinutes(5);
                }).ConfigurePrimaryHttpMessageHandler(c => buidler.GetService<LoggingHttpClientHandler>());

            return services;
        }
    }
}
