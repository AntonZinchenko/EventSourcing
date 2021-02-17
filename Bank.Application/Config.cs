using BankAccount.Application.Commands;
using BankAccount.Application.Processing;
using BankAccount.MaterializedView.Projections;
using BankAccount.Storage;
using Marten;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeedWorks.Core.Storage;
using System.Reflection;

namespace BankAccount.Application
{
    public static class Config
    {
        public static void ConfigApplication(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddMarten(config, options => ConfigureMarten(options));

            services.AddFluentValidation(new[] { typeof(CreateBankAccountValidator).GetTypeInfo().Assembly });
            services.AddScoped<IRepository<DomainModel.BankAccount>, MartenRepository<DomainModel.BankAccount>>();

            services.AddMediatR(typeof(CommandHandler));
        }

        public static void ConfigureMarten(StoreOptions options)
        {
            options.Events.InlineProjections.AggregateStreamsWith<DomainModel.BankAccount>();
            options.Events.InlineProjections.Add<BankAccountDetailsViewProjection>();
            options.Events.InlineProjections.Add<BankAccountShortInfoViewProjection>();
        }
    }
}
