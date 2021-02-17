using SeedWorks.Core.Events;
using System;

namespace Transfer.Contracts.Events
{
    public class ActionFaulted : ISagaEvent
    {
        public ActionFaulted(string reason, Guid correlationId, string commandName)
        {
            Reason = reason;
            CorrelationId = correlationId;
            CommandName = commandName;
        }

        /// <summary>
        /// Причина сбоя операции.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Наименование команды.
        /// </summary>
        public string CommandName { get; set; }
    }
}
