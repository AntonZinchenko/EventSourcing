using SeedWorks.Core.Events;
using System;

namespace Bank.Orchestrators.Contracts
{
    public interface ISumTransferStarted: ISagaEvent
    {
        /// <summary>
        /// Идентификатор счета с которого производится списание денежных средств.
        /// </summary>
        public Guid SourceAccountId { get; set; }

        /// <summary>
        /// Идентификатор счета на который производится зачисление денежных средств.
        /// </summary>
        public Guid TargetAccountId { get; set; }

        /// <summary>
        /// Сумма перевода.
        /// </summary>
        public decimal Sum { get; set; }
    }
}
