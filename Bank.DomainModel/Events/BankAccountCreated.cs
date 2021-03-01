using Newtonsoft.Json;
using System;

namespace BankAccount.DomainModel.Events
{
    /// <summary>
    /// Событие открытия расчетного счета.
    /// </summary>
    public class BankAccountCreated : BaseAccountEvent
    {
        [JsonConstructor]
        public BankAccountCreated(Guid accountId, int accountVersion, DateTime created, Guid correlationId, string owner)
            : base(accountId, accountVersion, created, correlationId)
        {
            Owner = owner;
        }

        /// <summary>
        /// Имя владельца расчетного счета.
        /// </summary>
        public string Owner { get; }

        public static BankAccountCreated Create(Guid correlationId, string owner)
            => new BankAccountCreated(Guid.NewGuid(), 1, DateTime.Now, correlationId, owner);
    }
}
