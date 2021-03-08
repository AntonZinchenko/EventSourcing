using Graphql.Graphql.Interfaces;
using Graphql.Types.Accounts.Types;
using HotChocolate.Types;
using SeedWorks;
using System.Threading;
using Transfer.Contracts.Types;

namespace Graphql.Types.Transfers.Types
{
    public class TransferType
        : ObjectType<TransferView>
    {
        private readonly IBankAccountClient _accountClient;

        public TransferType(IBankAccountClient accountClient)
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
                .Resolver(ctx => ctx.Parent<TransferView>()
                    .PipeTo(acc => _accountClient.GetBankAccountByIdAsync(acc.SourceAccountId, acc.SourceAccountVersion, CancellationToken.None)))
                .Description("Банковский счет с которого производится списание денежных средств.");

            descriptor.Field("TargetAccount")
                .Type<NonNullType<AccountType>>()
                .Resolver(ctx => ctx.Parent<TransferView>()
                    .PipeTo(acc => _accountClient.GetBankAccountByIdAsync(acc.TargetAccountId, acc.TargetAccountVersion, CancellationToken.None)))
                .Description("Банковский счет на который производится зачисление денежных средств.");

            descriptor.Field(t => t.Comment)
                .Type<NonNullType<StringType>>()
                .Description("Комментарий.");
        }
    }
}
