using BankAccount.Contracts.Requests;
using System;

namespace Gateway.Graphql.Models
{
    public class PerformDepositeRequestModel: PerformDepositeRequest
    {
        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; set; }
    }
}
