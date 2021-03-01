using BankAccount.Contracts.Views;
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
        public GetBankAccountShortInfoQuery(Guid accountId, int accountVersion = default)
        {
            AccountId = accountId;
            AccountVersion = accountVersion;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }

        /// <summary>
        /// Версия агрегата расчетного счета.
        /// </summary>
        public int AccountVersion { get; }
    }

    public class GetBankAccountShortInfoValidator : AbstractValidator<GetBankAccountShortInfoQuery>
    {
        public GetBankAccountShortInfoValidator()
        {
            RuleFor(request => request.AccountId).NotEmpty();
        }
    }
}
