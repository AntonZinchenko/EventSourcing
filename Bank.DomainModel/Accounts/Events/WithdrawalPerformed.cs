using Newtonsoft.Json;
using SeedWorks.Core.Events;
using System;

namespace Bank.DomainModel.Accounts.Events
{
    /// <summary>
    /// Событие выполнения списание с расчетного счета.
    /// </summary>
    public class WithdrawalPerformed : IEvent
    {
        [JsonConstructor]
        public WithdrawalPerformed(Guid accountId, decimal sum, DateTime created)
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
        /// Сумма списания.
        /// </summary>
        public decimal Sum { get; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime Created { get; }

        public static WithdrawalPerformed Create(Guid accountId, decimal sum)
            => new WithdrawalPerformed(accountId, sum, DateTime.Now);
    }
}
