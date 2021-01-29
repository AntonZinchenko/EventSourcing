using Bank.Orchestrators.Transfer;
using Bank.Orchestrators.Transfer.Flat;
using Bank.Orchestrators.Transfer.RoutingSlip.Activities;
using Bank.Orchestrators.Transfer.RoutingSlip.Activities.ProcessOutflow;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Saga;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeedWorks.Core.Events;
using System;
using GreenPipes;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Bank.Orchestrators
{
    public static class Config
    {
        public static void ConfigOrchestrators(this IServiceCollection services, IConfiguration config)
        {
            services.AddMassTransit(x =>
            {
                x.AddApplication();
                x.SetKebabCaseEndpointNameFormatter();
                x.AddSagaStateMachine<TransferStateMachine, TransferState>(sagaConfig =>
                {
                    sagaConfig.UseMessageRetry(r => r.Immediate(5));
                    sagaConfig.UseInMemoryOutbox();
                })
                .InMemoryRepository();
                 
                x.AddActivity(typeof(ProcessOutflowActivity));
                x.AddActivity(typeof(ProcessInflowActivity));

                var provider = x.Collection.BuildServiceProvider();

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.AutoStart = true;
                    cfg.ReceiveEndpoint(e =>
                    {
                        e.UseInMemoryOutbox();

                        var rep = provider.GetRequiredService<ISagaRepository<TransferState>>();

                        e.StateMachineSaga(provider.GetRequiredService<TransferStateMachine>(),
                             rep, sagaConfig =>
                             {
                                 sagaConfig.UseMessageRetry(r => r.Immediate(5));
                                 sagaConfig.UseInMemoryOutbox();
                             });

                        e.ConfigureActivityExecute(context, typeof(ProcessOutflowActivity), new Uri($"queue:process-outflow_execute"));
                        e.ConfigureActivityExecute(context, typeof(ProcessInflowActivity), new Uri($"queue:process-inflow_execute"));
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddSingleton<IHostedService, BusHostedService>();
        }

        public static IServiceCollectionBusConfigurator AddApplication(this IServiceCollectionBusConfigurator services)
        {
            services.AddConsumers(Assembly.GetExecutingAssembly());
            services.AddActivities(Assembly.GetExecutingAssembly());

            return services;
        }
    }

    public class BusHostedService : IHostedService
    {
        private readonly IBusControl _busControl;

        public BusHostedService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _busControl.StopAsync(cancellationToken);
        }
    }
}
