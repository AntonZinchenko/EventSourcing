using BankAccount.Contracts.Views;
using HotChocolate.Types;

namespace Gateway.Graphql.Types
{
    public class CashFlowItemType
        : ObjectType<CashFlowItem>
    {
        protected override void Configure(IObjectTypeDescriptor<CashFlowItem> descriptor)
        {
            descriptor.BindFieldsExplicitly()
                .Description("Операция.");

            descriptor.Field(t => t.Sum)
                .Type<NonNullType<DecimalType>>()
                .Description("Сумма проводки.");

            descriptor.Field(t => t.Date)
                .Type<NonNullType<DateType>>()
                .Description("Дата проведения операции.");
        }
    }
}
