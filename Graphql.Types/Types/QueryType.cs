using HotChocolate.Types;
using Gateway.Graphql.Models;

namespace Gateway.Graphql.Types
{
    public class QueryType
        : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(t => t.GetTransactions())
                .Name("getActiveTransactions")
                .Description("Получить список активных транзакций.");

            descriptor.Field(t => t.GetTransaction(default))
                .Name("getTransaction")
                .Argument("transactionId", a => a.Type<NonNullType<UuidType>>().Description("Идентификатор перевода."))
                .Description("Получить данные денежного перевода.");

            descriptor.Field(t => t.GetAccountInfo(default))
                .Argument("accountId", a => a.Type<NonNullType<UuidType>>().Description("Идентификатор расчетного счета."))
                .Name("getAccount")
                .Description("Получить выписку банковского счета.");
        }
    }
}
