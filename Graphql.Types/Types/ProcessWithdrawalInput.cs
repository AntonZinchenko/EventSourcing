using Gateway.Graphql.Models;
using HotChocolate.Types;

namespace Gateway.Graphql.Types
{
    public class ProcessWithdrawalInput
        : InputObjectType<PerformWithdrawalRequestModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PerformWithdrawalRequestModel> descriptor)
        {
            descriptor.Field(t => t.AccountId)
                .Type<NonNullType<StringType>>()
                .Description("Идентификатор расчетного счета.");

            descriptor.Field(t => t.Sum)
                .Type<NonNullType<DecimalType>>()
                .Description("Сумма списания.");
        }
    }
}
