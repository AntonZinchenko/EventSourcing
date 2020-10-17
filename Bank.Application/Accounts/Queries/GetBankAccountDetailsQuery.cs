using Bank.MaterializedView.Accounts.Views;
using FluentValidation;
using MediatR;
using System;

namespace Bank.Application.Accounts.Queries
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
