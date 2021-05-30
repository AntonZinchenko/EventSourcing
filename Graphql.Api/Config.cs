using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using Graphql.Types.Loaders;
using Graphql.Types.Accounts;
using Graphql.Types.Transfers;
using Graphql.Types.Accounts.Types;
using Graphql.Types.Transfers.Types;

namespace Gateway.Graphql
{
    public static class Config
    {
        public static IServiceCollection RegistergGraphqlTypes(this IServiceCollection services)
        {
            services
                .AddGraphQLServer()
                .AddQueryType(d => d.Name("Query"))
                    .AddTypeExtension<AccountQueries>()
                        .AddType<AccountType>()
                        .AddType<AccountDetailsType>()
                        .AddType<CashFlowItemType>()
                    .AddTypeExtension<TransferQueries>()
                        .AddType<TransferType>()
                .AddMutationType(d => d.Name("Mutation"))
                    .AddTypeExtension<AccountMutations>()
                        .AddType<CreateBankAccountInput>()
                        .AddType<ProcessDepositeInput>()
                        .AddType<ProcessWithdrawalInput>()
                        .AddType<RenameOwnerInput>()
                    .AddTypeExtension<TransferMutations>()
                        .AddType<ExecuteTransferInput>()
               .AddSubscriptionType(d => d.Name("Subscription"))
                    .AddTypeExtension<TransferSubscriptions>()
               .AddDataLoader<BankAccountByIdDataLoader>()
               .AddDataLoader<TransactionByIdDataLoader>();

            return services;
        }
    }
}
