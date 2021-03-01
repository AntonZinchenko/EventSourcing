using Newtonsoft.Json;
using SeedWorks.Core.Events;
using System;
using Transfer.Contracts.Events;

namespace BankAccount.DomainModel.Events
{
    /// <summary>
    /// Событие выполнения списание с расчетного счета.
    /// </summary>
    public class WithdrawalPerformed : BaseAccountEvent, ISagaEvent, IWithdrawalPerformed
    {
        [JsonConstructor]
        public WithdrawalPerformed(Guid accountId, int accountVersion, DateTime created, Guid correlationId, decimal sum)
            : base(accountId, accountVersion, created, correlationId)
        {
            Sum = sum;
        }

        /// <summary>
        /// Сумма списания.
        /// </summary>
        public decimal Sum { get; }

        public static WithdrawalPerformed Create(Guid accountId, int accountVersion, Guid correlationId, decimal sum)
            => new WithdrawalPerformed(accountId, accountVersion, DateTime.Now, correlationId, sum);
    }
}
