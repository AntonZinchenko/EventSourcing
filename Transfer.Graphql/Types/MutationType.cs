using HotChocolate.Types;
using Gateway.Graphql.Models;

namespace Gateway.Graphql.Types
{
    public class MutationType
        : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field(t => t.ExecuteTransferAsync(default))
                .Type<NonNullType<TransactionType>>()
                .Argument("request", a => a.Type<NonNullType<ExecuteTransferInput>>())
                .Description("Выполнить денежный перевод между счетами.");
        }
    }
}
