using System;

namespace SeedWorks.Core.Events
{
    public interface ISagaEvent
    {
        Guid CorrelationId { get; set; }
    }

    public interface ISagaRequest
    {
        Guid CorrelationId { get; }
    }
}