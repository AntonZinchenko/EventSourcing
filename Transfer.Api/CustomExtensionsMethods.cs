using AutoMapper;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.Saga;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SeedWorks;
using SeedWorks.HttpClients;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Transfer.Application.Interfaces;
using Transfer.Application.Mappers;
using Transfer.Application.Orchestrators;
using Transfer.Application.Orchestrators.Activities.ProcessOutflow;
using Transfer.Application.Orchestrators.RoutingSlip;
using Transfer.Application.Orchestrators.RoutingSlip.Activities;
using Transfer.Contracts.Events;
using Transfer.Infrastructure.HttpClients;
using Transfer.Storage;

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

        public static void ConfigOrchestrators(this IServiceCollection services, IConfiguration config)
        {
            services.InitAutoMapper()
                .AddMassTransit(x =>
                {
                    x.AddServiceClient();
                    x.AddRequestClient<ISumTransferStarted>();
                    x.AddConsumers(Assembly.GetExecutingAssembly());
                    x.AddActivities(Assembly.GetExecutingAssembly());
                    x.SetKebabCaseEndpointNameFormatter();
                    x.AddSagaStateMachine<TransferStateMachine, TransferState>(sagaConfig =>
                    {
                        sagaConfig.UseInMemoryOutbox();
                    })
                    .EntityFrameworkRepository(r =>
                    {
                        r.ConcurrencyMode = ConcurrencyMode.Pessimistic;

                        r.AddDbContext<DbContext, TransferDbContext>((provider, optionsBuilder) =>
                        {
                            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                        });

                        r.LockStatementProvider =
                            new CustomSqlLockStatementProvider("select * from {0}.{1} WITH (UPDLOCK, ROWLOCK) WHERE TransferId = @p0");
                    });

                    x.AddActivity(typeof(ProcessOutflowActivity));
                    x.AddActivity(typeof(ProcessInflowActivity));

                    var provider = x.Collection.BuildServiceProvider();

                    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var settings = provider.GetService<IOptions<RabbitOptions>>().Value;
                        cfg.Host(settings.Host, "/", h =>
                        {
                            h.Username(settings.Login);
                            h.Password(settings.Password);
                        });

                        cfg.AutoStart = true;
                        cfg.ReceiveEndpoint(e =>
                        {
                            e.UseInMemoryOutbox();

                            e.StateMachineSaga(
                                provider.GetRequiredService<TransferStateMachine>(),
                                provider.GetRequiredService<ISagaRepository<TransferState>>(),
                                sagaConfig =>
                                {
                                    sagaConfig.UseInMemoryOutbox();
                                });
                        });
                    }));
                });

            services.AddSingleton<IHostedService, BusHostedService>();

            services.AddDbContext<TransferDbContext>();
            services.AddScoped<IQueryRepository<TransferState>, QueryRepository<TransferState>>();
        }

        public static IServiceCollection InitAutoMapper(this IServiceCollection services)
            => new MapperConfiguration(x => x.AddProfile<TransferStateToCommandProfile>())
                .PipeTo(config => services.AddSingleton(config.CreateMapper()));
    }
}
