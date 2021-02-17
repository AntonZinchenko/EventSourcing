using FluentValidation;
using MediatR;
using SeedWorks.Core.Events;
using System;

namespace BankAccount.Application.Commands
{
    /// <summary>
    /// Команда списания с расчетного счета.
    /// </summary>
    public class PerformWithdrawalCommand : CorrelationByRequest<Unit>, ISagaRequest
    {
        public PerformWithdrawalCommand(Guid accountId, decimal sum, Guid correlationId)
            : base(correlationId)
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
