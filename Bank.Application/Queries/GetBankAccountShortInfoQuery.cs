using BankAccount.MaterializedView.Views;
using FluentValidation;
using MediatR;
using System;

namespace BankAccount.Application.Queries
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
