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
              //  .AddGraphQLSubscriptions()
                .AddGraphQL(
                    SchemaBuilder.New()
                        .AddQueryType<QueryType>()
                        .AddType<TransactionType>()
                        .AddType<AccountType>()
                        .AddMutationType<MutationType>()
                        .AddType<ExecuteTransferInput>()
                        .AddSubscriptionType(d => d.Name("Subscription"))
                        .AddType<SubscriptionType>()
                        .BindClrType<string, StringType>());

            return services;
        }
    }
}
