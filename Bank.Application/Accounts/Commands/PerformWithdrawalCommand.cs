using FluentValidation;
using MediatR;
using System;

namespace Bank.Application.Accounts.Commands
{
    /// <summary>
    /// Команда списания с расчетного счета.
    /// </summary>
    public class PerformWithdrawalCommand : IRequest<Unit>
    {
        public PerformWithdrawalCommand(Guid accountId, decimal sum)
        {
            AccountId = accountId;
            Sum = sum;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }

        /// <summary>
        /// Сумма списания.
        /// </summary>
        public decimal Sum { get; }
    }

    public class PerformWithdrawalValidator : AbstractValidator<PerformWithdrawalCommand>
    {
        public PerformWithdrawalValidator()
        {
            RuleFor(request => request.AccountId).NotEmpty();
            RuleFor(request => request.Sum).GreaterThan(0);
        }
    }
}
