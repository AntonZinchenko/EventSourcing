﻿using BankAccount.Contracts.Requests;
using HotChocolate.Types;

namespace Graphql.Types.Accounts.Types
{
    public class CreateBankAccountInput
        : InputObjectType<CreateBankAccountRequest>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateBankAccountRequest> descriptor)
        {
            descriptor.Field(t => t.Owner)
                .Type<NonNullType<StringType>>()
                .Description("Имя владельца расчетного счета.");
        }
    }
}