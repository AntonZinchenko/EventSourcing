using SeedWorks.Core.Events;
using System;

namespace Transfer.Contracts.Events
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
