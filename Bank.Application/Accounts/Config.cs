using Bank.Application.Accounts.Commands;
using Bank.Application.Accounts.Queries;
using Bank.DomainModel.Accounts;
using Bank.MaterializedView.Accounts.Projections;
using Bank.MaterializedView.Accounts.Views;
using Bank.Storage;
using Marten;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SeedWorks.Core.Storage;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bank.Application.Accounts
{
    public static class Config
    {
        public static void AddBankAccount(this IServiceCollection services)
        {
            services.AddFluentValidation(new[] { typeof(CreateBankAccountValidator).GetTypeInfo().Assembly });
            services.AddScoped<IRepository<BankAccount>, MartenRepository<BankAccount>>();

            // services.AddMediatR(typeof(BankAccountCommandHandler));
            services.AddScoped<IRequestHandler<ChangeOwnerCommand, Unit>, BankAccountCommandHandler>();
            services.AddScoped<IRequestHandler<CreateBankAccountCommand, Guid>, BankAccountCommandHandler>();
            services.AddScoped<IRequestHandler<PerformDepositeCommand, Unit>, BankAccountCommandHandler>();
            services.AddScoped<IRequestHandler<PerformWithdrawalCommand, Unit>, BankAccountCommandHandler>();
            services.AddScoped<IRequestHandler<RebuildAccountsViewsCommand, Unit>, BankAccountCommandHandler>();
            services.AddScoped<IRequestHandler<TransferBetweenAccountsCommand, Unit>, BankAccountCommandHandler>();

            services.AddScoped<IRequestHandler<GetBankAccountDetailsQuery, BankAccountDetailsView>, BankAccountQueryHandler>();
            services.AddScoped<IRequestHandler<GetBankAccountShortInfoQuery, BankAccountShortInfoView>, BankAccountQueryHandler>();
            services.AddScoped<IRequestHandler<GetBankAccountsQuery, Dictionary<Guid, string>>, BankAccountQueryHandler>();
        }

        public static void ConfigureMarten(StoreOptions options)
        {
            options.Events.InlineProjections.AggregateStreamsWith<BankAccount>();
            options.Events.InlineProjections.Add<BankAccountDetailsViewProjection>();
            options.Events.InlineProjections.Add<BankAccountShortInfoViewProjection>();
        }
    }
}
