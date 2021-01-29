﻿using Automatonymous;
using System;

namespace Bank.Orchestrators.Transfer
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