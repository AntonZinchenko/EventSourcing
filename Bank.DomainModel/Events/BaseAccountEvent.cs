using Newtonsoft.Json;
using SeedWorks.Core.Events;
using System;

namespace BankAccount.DomainModel.Events
{
    public class BaseAccountEvent : IEvent
    {
        [JsonConstructor]
        public BaseAccountEvent(Guid accountId, int accountVersion, DateTime created, Guid correlationId)
        {
            AccountId = accountId;
            AccountVersion = accountVersion;
            Created = created;
            CorrelationId = correlationId;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }

        /// <summary>
        /// Версия расчетного счета.
        /// </summary>
        public int AccountVersion { get; }

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
