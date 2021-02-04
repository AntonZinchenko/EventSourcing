using Bank.Application.Accounts.Commands;
using Bank.DomainModel.Accounts;
using Bank.MaterializedView.Accounts.Projections;
using Bank.Storage;
using Marten;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SeedWorks.Core.Storage;
using System.Reflection;

namespace Bank.Application.Accounts
{
    public static class Config
    {
        public static void AddBankAccount(this IServiceCollection services)
        {
            services.AddFluentValidation(new[] { typeof(CreateBankAccountValidator).GetTypeInfo().Assembly });
            services.AddScoped<IRepository<BankAccount>, MartenRepository<BankAccount>>();

            services.AddMediatR(typeof(BankAccountCommandHandler));
        }

        public static void ConfigureMarten(StoreOptions options)
        {
            options.Events.InlineProjections.AggregateStreamsWith<BankAccount>();
            options.Events.InlineProjections.Add<BankAccountDetailsViewProjection>();
            options.Events.InlineProjections.Add<BankAccountShortInfoViewProjection>();
        }
    }
}
