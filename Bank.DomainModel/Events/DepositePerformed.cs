using Newtonsoft.Json;
using SeedWorks.Core.Events;
using System;
using Transfer.Contracts.Events;

namespace BankAccount.DomainModel.Events
{
    /// <summary>
    /// Событие выполнения начисление депозитных процентов.
    /// </summary>
    public class DepositePerformed : BaseAccountEvent, ISagaEvent, IDepositePerformed
    {
        [JsonConstructor]
        public DepositePerformed(Guid accountId, int accountVersion, DateTime created, Guid correlationId, decimal sum)
            : base(accountId, accountVersion, created, correlationId)
        {
            Sum = sum;
        }

        /// <summary>
        /// Сумма проводки.
        /// </summary>
        public decimal Sum { get; }

        public static DepositePerformed Create(Guid accountId, int accountVersion, Guid correlationId, decimal sum)
            => new DepositePerformed(accountId, accountVersion, DateTime.Now, correlationId, sum);
    }
}
