using Newtonsoft.Json;
using SeedWorks.Core.Events;
using System;

namespace BankAccount.DomainModel.Events
{
    public class BaseAccountEvent : IEvent
    {
        [JsonConstructor]
        public BaseAccountEvent(Guid accountId, DateTime created, Guid correlationId)
        {
            AccountId = accountId;
            Created = created;
            CorrelationId = correlationId;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime Created { get; }

        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        public Guid CorrelationId { get; set; }
    }
}
