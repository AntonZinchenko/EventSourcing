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
            
            descriptor.Field(t => t.CreateBankAccountAsync(default))
                .Type<NonNullType<AccountDetailsType>>()
                .Argument("request", a => a.Type<NonNullType<CreateBankAccountInput>>())
                .Description("Открыть расчетный счет.");
          
          descriptor.Field(t => t.RenameOwnerAsync(default))
              .Type<NonNullType<AccountDetailsType>>()
              .Argument("request", a => a.Type<NonNullType<RenameOwnerInput>>())
              .Description("Сменить имя владельца расчетного счета.");

          descriptor.Field(t => t.ProcessWithdrawalAsync(default))
              .Type<NonNullType<AccountDetailsType>>()
              .Argument("request", a => a.Type<NonNullType<ProcessWithdrawalInput>>())
              .Description("Выполнить списание денежных средств.");

          descriptor.Field(t => t.ProcessDepositeAsync(default))
              .Type<NonNullType<AccountDetailsType>>()
              .Argument("request", a => a.Type<NonNullType<ProcessDepositeInput>>())
              .Description("Выполнить зачисление денежных средств.");
        }
    }
}
