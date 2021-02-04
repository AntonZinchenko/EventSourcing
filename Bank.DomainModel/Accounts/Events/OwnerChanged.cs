using Newtonsoft.Json;
using System;

namespace Bank.DomainModel.Accounts.Events
{
    /// <summary>
    /// Событие переоформления счет на другого пользователя.
    /// </summary>
    public class OwnerChanged : BaseAccountEvent
    {
        [JsonConstructor]
        public OwnerChanged(Guid accountId, DateTime created, Guid correlationId, string newOwner)
            : base(accountId, created, correlationId)
        {
            NewOwner = newOwner;
        }

        /// <summary>
        /// Имя нового владельца.
        /// </summary>
        public string NewOwner { get; }

        public static OwnerChanged Create(Guid accountId, Guid correlationId, string newOwner)
            => new OwnerChanged(accountId, DateTime.Now, correlationId, newOwner);
    }
}
