using System;

namespace SeedWorks
{
    public interface IExecutionContextAccessor
    {
        Guid CorrelationId { get; }
    }
}
