using BankAccount.Contracts.Views;
using HotChocolate.Types;

namespace Graphql.Types.Accounts.Types
{
    public class AccountType
        : ObjectType<BankAccountShortInfoView>
    {
        protected override void Configure(IObjectTypeDescriptor<BankAccountShortInfoView> descriptor)
        {
            descriptor.BindFieldsExplicitly()
                .Name("BankAccount")
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

        }
    }
}
