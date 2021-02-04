using System;

namespace Bank.Orchestrators.Transfer.RoutingSlip.Activities
{
    public interface ProcessInflowArguments
    {
        /// <summary>
        /// Идентификатор счета.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Сумма перевода.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        public Guid CorrelationId { get; set; }
    }
}
