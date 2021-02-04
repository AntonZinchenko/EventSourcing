using SeedWorks.Core.Events;
using System;

namespace Bank.Orchestrators.Contracts
{
    public class StartProcessing : ISagaRequest
    {
        public StartProcessing(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        public Guid CorrelationId { get; }
    }
}
