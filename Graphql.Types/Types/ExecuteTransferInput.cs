using HotChocolate.Types;
using Transfer.Contracts.Requests;

namespace Gateway.Graphql.Types
{
    public class ExecuteTransferInput
        : InputObjectType<TransferRequest>
    {
        protected override void Configure(IInputObjectTypeDescriptor<TransferRequest> descriptor)
        {
            descriptor.Field(t => t.SourceAccountId)
                .Type<NonNullType<UuidType>>()
                .Description("Идентификатор счета с которого производится списание денежных средств.");

            descriptor.Field(t => t.TargetAccountId)
                .Type<NonNullType<StringType>>()
                .Description("Идентификатор счета на который производится зачисление денежных средств.");

            descriptor.Field(t => t.Sum)
                .Type<NonNullType<DecimalType>>()
                .Description("Сумма перевода.");
        }
    }
}
