using Newtonsoft.Json;
using SeedWorks.Core.Events;
using System;

namespace Bank.DomainModel.Accounts.Events
{
    /// <summary>
    /// Событие переоформления счет на другого пользователя.
    /// </summary>
    public class OwnerChanged : IEvent
    {
        [JsonConstructor]
        public OwnerChanged(Guid accountId, string newOwner, DateTime created)
        {
            AccountId = accountId;
            NewOwner = newOwner;
            Created = created;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }

        /// <summary>
        /// Имя нового владельца.
        /// </summary>
        public string NewOwner { get; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime Created { get; }

        public static OwnerChanged Create(Guid accountId, string newOwner)
            => new OwnerChanged(accountId, newOwner, DateTime.Now);
    }
}
