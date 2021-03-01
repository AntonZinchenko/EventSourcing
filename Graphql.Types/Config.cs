using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using Gateway.Graphql.Types;

namespace Gateway.Graphql
{
    public static class Config
    {
        public static IServiceCollection RegistergGraphqlTypes(this IServiceCollection services)
        {
            services
                .AddInMemorySubscriptions()
                .AddGraphQL(
                    SchemaBuilder.New()
                        .AddQueryType<QueryType>()
                        .AddType<TransactionType>()
                        .AddType<AccountType>()
                        .AddType<AccountDetailsType>()
                        .AddMutationType<MutationType>()
                        .AddType<ExecuteTransferInput>()
                        .AddType<CreateBankAccountInput>()
                        .AddType<ProcessDepositeInput>()
                        .AddType<ProcessWithdrawalInput>()
                        .AddType<RenameOwnerInput>()
                        .AddType<CashFlowItemType>()
                        .AddSubscriptionType(d => d.Name("Subscription"))
                        .AddType<SubscriptionType>()
                        .BindClrType<string, StringType>());

            return services;
        }
    }
}
