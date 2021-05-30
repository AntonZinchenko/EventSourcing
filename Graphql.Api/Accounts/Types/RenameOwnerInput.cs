using Graphql.Types.Accounts.Types.Models;
using HotChocolate.Types;

namespace Graphql.Types.Accounts.Types
{
    public class RenameOwnerInput
        : InputObjectType<ChangeOwnerRequestModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ChangeOwnerRequestModel> descriptor)
        {
            descriptor.Field(t => t.AccountId)
                .Type<NonNullType<StringType>>()
                .Description("Идентификатор расчетного счета.");

            descriptor.Field(t => t.NewOwner)
                .Type<NonNullType<StringType>>()
                .Description("Новое имя владельца расчетного счета.");
        }
    }
}
