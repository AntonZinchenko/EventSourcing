using BankAccount.MaterializedView.Views;
using FluentValidation;
using MediatR;
using System;

namespace BankAccount.Application.Queries
{
    /// <summary>
    /// Запрос детализации по расчетному счету.
    /// </summary>
    public class GetBankAccountDetailsQuery : IRequest<BankAccountDetailsView>
    {
        public GetBankAccountDetailsQuery(Guid accountId)
        {
            AccountId = accountId;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }
    }

    public class GetBankAccountDetailsValidator : AbstractValidator<GetBankAccountDetailsQuery>
    {
        public GetBankAccountDetailsValidator()
        {
            RuleFor(request => request.AccountId).NotEmpty();
        }
    }
}
