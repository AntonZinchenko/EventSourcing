using AutoMapper;
using BankAccount.Application.Mappers;
using BankAccount.Application.Processing;
using BankAccount.MaterializedView.Projections;
using BankAccount.Storage;
using Marten;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeedWorks;
using SeedWorks.Core.Storage;

namespace BankAccount.Application
{
    public static class Config
    {
        public static void ConfigApplication(this IServiceCollection services, IConfiguration config)
        {
            services.AddMarten(config, options => ConfigureMarten(options));

            services.AddScoped<IRepository<DomainModel.BankAccount>, MartenRepository<DomainModel.BankAccount>>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(SagaLoggingBehavior<,>))
                .ConfigMediatR(typeof(CommandHandler))
                .InitAutoMapper();
        }

        public static void ConfigureMarten(StoreOptions options)
        {
            options.Events.InlineProjections.AggregateStreamsWith<DomainModel.BankAccount>();
            options.Events.InlineProjections.Add<BankAccountDetailsViewProjection>();
            options.Events.InlineProjections.Add<BankAccountShortInfoViewProjection>();
        }

        public static IServiceCollection InitAutoMapper(this IServiceCollection services)
            => new MapperConfiguration(x => x.AddProfile<AggregateProfile>())
                .PipeTo(config => services.AddSingleton(config.CreateMapper()));
    }
}
