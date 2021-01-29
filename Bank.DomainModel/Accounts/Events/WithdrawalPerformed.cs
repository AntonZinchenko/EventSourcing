using Bank.Orchestrators.Contracts;
using Newtonsoft.Json;
using SeedWorks.Core.Events;
using System;

namespace Bank.DomainModel.Accounts.Events
{
    /// <summary>
    /// Событие выполнения списание с расчетного счета.
    /// </summary>
    public class WithdrawalPerformed : BaseAccountEvent, ISagaEvent, IWithdrawalPerformed
    {
        [JsonConstructor]
        public WithdrawalPerformed(Guid accountId, DateTime created, Guid correlationId, decimal sum)
            : base(accountId, created, correlationId)
        {
            Sum = sum;
        }

        /// <summary>
        /// Сумма списания.
        /// </summary>
        public decimal Sum { get; }

        public static WithdrawalPerformed Create(Guid accountId, Guid correlationId, decimal sum)
            => new WithdrawalPerformed(accountId, DateTime.Now, correlationId, sum);
    }
}
