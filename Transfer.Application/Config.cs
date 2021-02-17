using MassTransit;
using MassTransit.Saga;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GreenPipes;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SeedWorks;
using Transfer.Storage;
using Transfer.Application.Orchestrators.Flat;
using Transfer.Application.Orchestrators.Activities.ProcessOutflow;
using Transfer.Application.Orchestrators.RoutingSlip.Activities;
using Transfer.Application.Mappers;
using Transfer.Contracts.Events;
using Microsoft.Extensions.Options;
using Transfer.Application.Commands;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;

namespace Transfer.Application
{
    public static class Config
    {
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
                        sagaConfig.UseMessageRetry(r => r.Immediate(5));
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
                                    sagaConfig.UseMessageRetry(r => r.Immediate(5));
                                    sagaConfig.UseInMemoryOutbox();
                                });
                        });
                    }));
                });

            services.AddSingleton<IHostedService, BusHostedService>();

            services.AddFluentValidation(new[] { typeof(TransferBetweenAccountsCommandValidator).GetTypeInfo().Assembly });
            services.AddMediatR(typeof(CommandHandler));
        }

        public static IServiceCollection InitAutoMapper(this IServiceCollection services)
            => new MapperConfiguration(x => x.AddProfile<TransferStateToCommandProfile>())
                .PipeTo(config => services.AddSingleton(config.CreateMapper()));
    }

    public class BusHostedService : IHostedService
    {
        private readonly IBusControl _busControl;

        public BusHostedService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public Task StartAsync(CancellationToken cancellationToken)
            => _busControl.StartAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
            => _busControl.StopAsync(cancellationToken);
    }
}
