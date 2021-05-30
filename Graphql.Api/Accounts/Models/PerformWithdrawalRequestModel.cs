using BankAccount.Contracts.Requests;
using System;

namespace Graphql.Types.Accounts.Types.Models
{
    public class PerformWithdrawalRequestModel: PerformWithdrawalRequest
    {
        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; set; }
    }
}
