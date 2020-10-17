using System;
using System.Collections.Generic;
using System.Linq;
using SeedWorks.Core.Events;
using Newtonsoft.Json;
using SeedWorks.Validation;

namespace SeedWorks.Core.Aggregates
{
    public abstract class Aggregate: IAggregate
    {
        public Guid Id { get; protected set; }

        public int Version { get; protected set; }

        [JsonIgnore]
        private readonly List<IEvent> uncommittedEvents = new List<IEvent>();

        protected Aggregate() { }

        IEnumerable<IEvent> IAggregate.DequeueUncommittedEvents()
            => uncommittedEvents.ToList()
                .Do(dequeuedEvents => uncommittedEvents.Clear());

        protected void Enqueue(IEvent @event)
        {
            Version++;
            uncommittedEvents.Add(@event);
        }

        protected void CheckRules(params IBusinessRule[] rules) 
            => rules.FirstOrDefault(r => r.IsBroken())
                .Either(brokenRule => throw new BusinessRuleValidationException(brokenRule), _ => true);
    }
}
