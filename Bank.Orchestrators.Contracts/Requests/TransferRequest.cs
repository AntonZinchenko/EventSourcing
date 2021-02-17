using System;

namespace Transfer.Contracts.Requests
{
    public class TransferRequest
    {
        /// <summary>
        /// Идентификатор счета на который производится зачисление денежных средств.
        /// </summary>
        public Guid TargetAccountId { get; set; }

        /// <summary>
        /// Идентификатор счета с которого производится списание денежных средств.
        /// </summary>
        public Guid SourceAccountId { get; set; }

        /// <summary>
        /// Сумма перевода.
        /// </summary>
        public decimal Sum { get; set; }
    }
}
