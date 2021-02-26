using BankAccount.Contracts.Views;
using FluentValidation;
using MediatR;
using System;

namespace Graphql.Application.Queries
{
    /// <summary>
    /// Запрос полной выписки банковского счета.
    /// </summary>
    public class GetDetailedAccountInfoQuery : IRequest<BankAccountDetailsView>
    {
        public GetDetailedAccountInfoQuery(Guid accountId)
        {
            AccountId = accountId;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }
    }

    public class GetDetailedAccountInfoQueryValidator : AbstractValidator<GetDetailedAccountInfoQuery>
    {
        public GetDetailedAccountInfoQueryValidator()
        {
            RuleFor(request => request.AccountId).NotEmpty();
        }
    }
}
