using Newtonsoft.Json;
using System;

namespace BankAccount.DomainModel.Events
{
    /// <summary>
    /// Событие переоформления счет на другого пользователя.
    /// </summary>
    public class OwnerChanged : BaseAccountEvent
    {
        [JsonConstructor]
        public OwnerChanged(Guid accountId, int accountVersion, DateTime created, Guid correlationId, string newOwner)
            : base(accountId, accountVersion, created, correlationId)
        {
            NewOwner = newOwner;
        }

        /// <summary>
        /// Имя нового владельца.
        /// </summary>
        public string NewOwner { get; }

        public static OwnerChanged Create(Guid accountId, int accountVersion, Guid correlationId, string newOwner)
            => new OwnerChanged(accountId, accountVersion, DateTime.Now, correlationId, newOwner);
    }
}
