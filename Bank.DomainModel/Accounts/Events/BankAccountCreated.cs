using Newtonsoft.Json;
using System;

namespace Bank.DomainModel.Accounts.Events
{
    /// <summary>
    /// Событие открытия расчетного счета.
    /// </summary>
    public class BankAccountCreated : BaseAccountEvent
    {
        [JsonConstructor]
        public BankAccountCreated(Guid accountId, DateTime created, Guid correlationId, string owner)
            : base(accountId, created, correlationId)
        {
            Owner = owner;
        }

        /// <summary>
        /// Имя владельца расчетного счета.
        /// </summary>
        public string Owner { get; }

        public static BankAccountCreated Create(Guid correlationId, string owner)
            => new BankAccountCreated(Guid.NewGuid(), DateTime.Now, correlationId, owner);
    }
}
