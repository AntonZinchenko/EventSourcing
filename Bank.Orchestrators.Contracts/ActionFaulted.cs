using SeedWorks.Core.Events;
using System;

namespace Bank.Orchestrators.Contracts
{
    public class ActionFaulted : ISagaEvent
    {
        public ActionFaulted(string reason, Guid correlationId, string actionName)
        {
            Reason = reason;
            CorrelationId = correlationId;
            ActionName = actionName;
        }

        public string Reason { get; set; }
        public Guid CorrelationId { get; set; }
        public string ActionName { get; set; }
    }
}
