using SeedWorks.Core.Events;
using System;

namespace Bank.Orchestrators.Contracts
{
    public class TransferSubmitted : ISagaEvent
    {
        public TransferSubmitted(Guid sourceAccountId, Guid targetAccountId, decimal sum, Guid correlationId)
        {
            SourceAccountId = sourceAccountId;
            TargetAccountId = targetAccountId;
            Sum = sum;
            CorrelationId = correlationId;
        }

        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Идентификатор счета с которого производится списание денеждных средств.
        /// </summary>
        public Guid SourceAccountId { get; set; }

        /// <summary>
        /// Идентификатор счета на который производится зачисление денеждных средств.
        /// </summary>
        public Guid TargetAccountId { get; set; }

        /// <summary>
        /// Сумма перевода.
        /// </summary>
        public decimal Sum { get; set; }
    }
}
