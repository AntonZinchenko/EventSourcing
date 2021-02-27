using Graphql.Graphql.Interfaces;
using HotChocolate.Types;
using SeedWorks;
using Transfer.Contracts.Types;

namespace Gateway.Graphql.Types
{
    public class TransactionType
        : ObjectType<TransferView>
    {
        private readonly IBankAccountClient _accountClient;

        public TransactionType(IBankAccountClient accountClient)
        {
            _accountClient = accountClient;
        }

        protected override void Configure(IObjectTypeDescriptor<TransferView> descriptor)
        {
            descriptor.BindFieldsExplicitly()
                .Name("Transfer")
                .Description("Банковский перевод между счетами.");

            descriptor.Field(t => t.Id)
                .Type<NonNullType<UuidType>>()
                .Description("Идентификатор денежного перевода.");

            descriptor.Field(t => t.Sum)
                .Type<NonNullType<DecimalType>>()
                .Description("Сумма перевода.");

            descriptor.Field("SourceAccount")
                .Type<NonNullType<AccountType>>()
                .Resolver(ctx => ctx.Parent<TransferView>().SourceAccountId
                    .PipeTo(accountId => _accountClient.GetDetailedInfo(accountId)))
                .Description("Банковский счет с которого производится списание денежных средств.");

            descriptor.Field("TargetAccount")
                .Type<NonNullType<AccountType>>()
                .Resolver(ctx => ctx.Parent<TransferView>().TargetAccountId
                    .PipeTo(accountId => _accountClient.GetDetailedInfo(accountId)))
                .Description("Банковский счет на который производится зачисление денежных средств.");
        }
    }
}
