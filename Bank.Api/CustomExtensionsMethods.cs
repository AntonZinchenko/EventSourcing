using BankAccount.Storage;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SeedWorks;
using SeedWorks.Core.Events;
using System;
using System.IO;
using System.Linq;

namespace BankAccount.Api
{
    public static class CustomExtensionsMethods
    {
        /// <summary>
        /// Конфигурирование Swagger.
        /// </summary>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
            => services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bank accounts", Version = "v1" });

                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(file => c.IncludeXmlComments(file));
            });

        /// <summary>
        /// Настройка шины сообщений.
        /// </summary>
        public static IServiceCollection ConfigRabbitBus(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = services.BuildServiceProvider();
            var settings = provider.GetService<IOptions<RabbitOptions>>().Value;

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(settings.Host, "/", h =>
                    {
                        h.Username(settings.Login);
                        h.Password(settings.Password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.TryAddScoped<IEventBus, EventBus>();

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
