using System;
using System.Collections.Generic;
using SeedWorks.Core.Events;

namespace SeedWorks.Core.Aggregates
{
    public interface IAggregate
    {
        Guid Id { get; }

        int Version { get; }

        IEnumerable<IEvent> DequeueUncommittedEvents();
    }
}
