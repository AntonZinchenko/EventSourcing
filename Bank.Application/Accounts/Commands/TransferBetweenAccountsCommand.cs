using FluentValidation;
using MediatR;
using SeedWorks;
using System;

namespace Bank.Application.Accounts.Commands
{
    public class TransferBetweenAccountsCommand : CorrelationByRequest<Unit>
    {
        public TransferBetweenAccountsCommand(Guid sourceAccountId, Guid targetAccountId, decimal sum, Guid correlationId)
            : base(correlationId)
        {
            SourceAccountId = sourceAccountId;
            TargetAccountId = targetAccountId;
            Sum = sum;
        }

        /// <summary>
        /// Идентификатор счета с которого производится списание денеждных средств.
        /// </summary>
        public Guid SourceAccountId { get; }

        /// <summary>
        /// Идентификатор счета на который производится зачисление денеждных средств.
        /// </summary>
        public Guid TargetAccountId { get; }

        /// <summary>
        /// Сумма перевода.
        /// </summary>
        public decimal Sum { get; }
    }

    public class TransferBetweenAccountsCommandValidator : AbstractValidator<TransferBetweenAccountsCommand>
    {
        public TransferBetweenAccountsCommandValidator()
        {
            RuleFor(request => request.SourceAccountId).NotEmpty();
            RuleFor(request => request.TargetAccountId).NotEmpty();
            RuleFor(request => request.Sum).GreaterThan(0);
        }
    }
}
