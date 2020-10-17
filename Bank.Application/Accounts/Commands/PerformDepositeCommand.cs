using FluentValidation;
using MediatR;
using System;

namespace Bank.Application.Accounts.Commands
{
    /// <summary>
    /// Команда начисления депозитных процентов.
    /// </summary>
    public class PerformDepositeCommand : IRequest<Unit>
    {
        public PerformDepositeCommand(Guid accountId, decimal sum)
        {
            AccountId = accountId;
            Sum = sum;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }
        
        /// <summary>
        /// Сумма проводки.
        /// </summary>
        public decimal Sum { get; }
    }

    public class PerformDepositeValidator : AbstractValidator<PerformDepositeCommand>
    {
        public PerformDepositeValidator()
        {
            RuleFor(request => request.AccountId).NotEmpty();
            RuleFor(request => request.Sum).GreaterThan(0);
        }
    }
}
