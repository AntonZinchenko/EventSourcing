using FluentValidation;
using SeedWorks;
using System;

namespace Transfer.Application.Commands
{
    public class TransferBetweenAccountsCommand : CorrelationByRequest<Guid>
    {
        public TransferBetweenAccountsCommand(Guid sourceAccountId, Guid targetAccountId, decimal sum, Guid correlationId)
            : base(correlationId)
        {
            SourceAccountId = sourceAccountId;
            TargetAccountId = targetAccountId;
            Sum = sum;
        }

        /// <summary>
        /// Идентификатор счета с которого производится списание денежных средств.
        /// </summary>
        public Guid SourceAccountId { get; }

        /// <summary>
        /// Идентификатор счета на который производится зачисление денежных средств.
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
