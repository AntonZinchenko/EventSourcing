using Newtonsoft.Json;
using SeedWorks.Core.Events;
using System;

namespace Bank.DomainModel.Accounts.Events
{
    /// <summary>
    /// Событие выполнения начисление депозитных процентов.
    /// </summary>
    public class DepositePerformed : IEvent
    {
        [JsonConstructor]
        public DepositePerformed(Guid accountId, decimal sum, DateTime created)
        {
            AccountId = accountId;
            Sum = sum;
            Created = created;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }

        /// <summary>
        /// Сумма проводки.
        /// </summary>
        public decimal Sum { get; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime Created { get; }

        public static DepositePerformed Create(Guid accountId, decimal sum)
            => new DepositePerformed(accountId, sum, DateTime.Now);
    }
}
