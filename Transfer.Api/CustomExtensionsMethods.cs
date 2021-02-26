using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SeedWorks;
using SeedWorks.HttpClients;
using System;
using System.IO;
using System.Linq;
using Transfer.Application.Interfaces;
using Transfer.Infrastructure;
using Transfer.Infrastructure.HttpClients;

namespace Transfer.Api
{
    public static class CustomExtensionsMethods
    {
        /// <summary>
        /// Конфигурирование Swagger.
        /// </summary>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
            => services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transfers", Version = "v1" });

                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(file => c.IncludeXmlComments(file));
            });

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

            return services;
        }

        /// <summary>
        /// Инициализация объектов предназначенных для работы с конфигом.
        /// </summary>
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
            => services.AddOptions()
               .Configure<RabbitOptions>(configuration.GetSection("Rabbit"));

        public static IServiceCollection AddContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();

            return services;
        }
    }
}
