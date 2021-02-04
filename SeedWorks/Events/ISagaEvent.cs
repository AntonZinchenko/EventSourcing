using System;

namespace SeedWorks.Core.Events
{
    public interface ISagaEvent
    {
        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        Guid CorrelationId { get; set; }
    }

    public interface ISagaRequest
    {
        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        Guid CorrelationId { get; }
    }
}