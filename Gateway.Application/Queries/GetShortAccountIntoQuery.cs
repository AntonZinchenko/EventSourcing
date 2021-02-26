using BankAccount.Contracts.Views;
using FluentValidation;
using MediatR;
using System;

namespace Graphql.Application.Queries
{
    /// <summary>
    /// Запрос выписки банковского счета.
    /// </summary>
    public class GetShortAccountIntoQuery : IRequest<BankAccountShortInfoView>
    {
        public GetShortAccountIntoQuery(Guid accountId)
        {
            AccountId = accountId;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }
    }

    public class GetAccountQueryValidator : AbstractValidator<GetShortAccountIntoQuery>
    {
        public GetAccountQueryValidator()
        {
            RuleFor(request => request.AccountId).NotEmpty();
        }
    }
}
