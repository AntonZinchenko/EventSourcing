using SeedWorks.Core.Events;
using System;

namespace Transfer.Contracts.Events
{
    public interface IWithdrawalPerformed : ISagaEvent
    {
        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        Guid AccountId { get; }

        /// <summary>
        /// Версия расчетного счета.
        /// </summary>
        int AccountVersion { get; }
    }
}
