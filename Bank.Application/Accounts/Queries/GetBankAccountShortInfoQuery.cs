using Bank.MaterializedView.Accounts.Views;
using FluentValidation;
using MediatR;
using System;

namespace Bank.Application.Accounts.Queries
{
    /// <summary>
    /// Запрос краткой информацию по расчетному счету.
    /// </summary>
    public class GetBankAccountShortInfoQuery : IRequest<BankAccountShortInfoView>
    {
        public GetBankAccountShortInfoQuery(Guid accountId)
        {
            AccountId = accountId;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }
    }

    public class GetBankAccountShortInfoValidator : AbstractValidator<GetBankAccountShortInfoQuery>
    {
        public GetBankAccountShortInfoValidator()
        {
            RuleFor(request => request.AccountId).NotEmpty();
        }
    }
}
