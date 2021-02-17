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
        public DepositePerformed(Guid accountId, DateTime created, Guid correlationId, decimal sum)
            : base(accountId, created, correlationId)
        {
            Sum = sum;
        }

        /// <summary>
        /// Сумма проводки.
        /// </summary>
        public decimal Sum { get; }

        public static DepositePerformed Create(Guid accountId, Guid correlationId, decimal sum)
            => new DepositePerformed(accountId, DateTime.Now, correlationId, sum);
    }
}
