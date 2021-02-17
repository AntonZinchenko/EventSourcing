using System;

namespace Transfer.Application.Orchestrators.RoutingSlip.Activities
{
    public interface ActivityLog
    {
        /// <summary>
        /// Целевой счет.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Сумма.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        public Guid CorrelationId { get; set; }
    }
}
