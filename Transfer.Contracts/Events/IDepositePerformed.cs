using SeedWorks.Core.Events;
using System;

namespace Transfer.Contracts.Events
{
    public interface IDepositePerformed : ISagaEvent
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
