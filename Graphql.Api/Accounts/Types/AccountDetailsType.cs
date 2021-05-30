using BankAccount.Contracts.Views;
using HotChocolate.Types;

namespace Graphql.Types.Accounts.Types
{
    public class AccountDetailsType
        : ObjectType<BankAccountDetailsView>
    {
        protected override void Configure(IObjectTypeDescriptor<BankAccountDetailsView> descriptor)
        {
            descriptor.BindFieldsExplicitly()
                .Name("BankAccountDetails")
                .Description("Выписка по расчетному счету.");

            descriptor.Field(t => t.Id)
                .Type<NonNullType<UuidType>>()
                .Description("Идентификатор расчетного счета.");

            descriptor.Field(t => t.Owner)
                .Type<NonNullType<StringType>>()
                .Description("Имя владельца.");

            descriptor.Field(t => t.Balance)
                .Type<NonNullType<DecimalType>>()
                .Description("Текущий баланс.");

            descriptor.Field(t => t.CashFlow)
                .Type<NonNullType<ListType<CashFlowItemType>>>()
                .Description("Движение денежных средств.");

        }
    }
}
