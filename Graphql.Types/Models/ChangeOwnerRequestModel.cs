﻿using BankAccount.Contracts.Requests;
using System;

namespace Gateway.Graphql.Models
{
    public class ChangeOwnerRequestModel: ChangeOwnerRequest
    {
        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; set; }
    }
}
