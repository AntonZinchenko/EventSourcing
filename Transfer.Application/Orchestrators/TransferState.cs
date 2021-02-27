using Automatonymous;
using System;

namespace Transfer.Application.Orchestrators
{
    public class TransferState : SagaStateMachineInstance
    {
        /// <summary>
        /// Текущее состояние.
        /// </summary>
        public string CurrentState { get; set; }

        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        public Guid CorrelationId { get; set; }

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

        /// <summary>
        /// Комментарий.
        /// </summary>
        public string Comment { get; set; }
    }
}
