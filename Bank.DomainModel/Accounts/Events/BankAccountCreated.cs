using Newtonsoft.Json;
using SeedWorks.Core.Events;
using System;

namespace Bank.DomainModel.Accounts.Events
{
    /// <summary>
    /// Событие открытия расчетного счета.
    /// </summary>
    public class BankAccountCreated : IEvent
    {
        [JsonConstructor]
        public BankAccountCreated(Guid accountId, string owner, DateTime created)
        {
            AccountId = accountId;
            Owner = owner;
            Created = created;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }

        /// <summary>
        /// Имя владельца расчетного счета.
        /// </summary>
        public string Owner { get; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime Created { get; }

        public static BankAccountCreated Create(string owner)
            => new BankAccountCreated(Guid.NewGuid(), owner, DateTime.Now);
    }
}
