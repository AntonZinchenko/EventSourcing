using Bank.Orchestrators.Transfer;
using Bank.Orchestrators.Transfer.Flat;
using Bank.Orchestrators.Transfer.RoutingSlip.Activities;
using Bank.Orchestrators.Transfer.RoutingSlip.Activities.ProcessOutflow;
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
using Bank.Orchestrators.Transfer.StatePersistence;
using AutoMapper;
using Bank.Orchestrators.Mappers;
using SeedWorks;

namespace Bank.Orchestrators
{
    public static class Config
    {
        public static void ConfigOrchestrators(this IServiceCollection services, IConfiguration config)
        {
            services.InitAutoMapper()
                .AddMassTransit(x =>
                {
                    x.AddConsumers(Assembly.GetExecutingAssembly());
                    x.AddActivities(Assembly.GetExecutingAssembly());
                    x.SetKebabCaseEndpointNameFormatter();
                    x.AddSagaStateMachine<TransferStateMachine, TransferState>(sagaConfig =>
                    {
                        // sagaConfig.UseMessageRetry(r => r.Immediate(5));
                        sagaConfig.UseInMemoryOutbox();
                    })
                    //.InMemoryRepository();
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

                    x.UsingInMemory((context, cfg) =>
                    {
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

                        cfg.ConfigureEndpoints(context);
                    });
                });

            services.AddSingleton<IHostedService, BusHostedService>();
        }

        public static IServiceCollection InitAutoMapper(this IServiceCollection services)
            => new MapperConfiguration(x =>
                {
                    x.AddProfile<ActivityToCommandProfile>();
                    x.AddProfile<TransferStateToCommandProfile>();
                }).PipeTo(config => services.AddSingleton(config.CreateMapper()));
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
