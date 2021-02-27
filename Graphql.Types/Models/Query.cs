using BankAccount.Contracts.Views;
using Graphql.Graphql.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transfer.Contracts.Types;

namespace Gateway.Graphql.Models
{
    public class Query
    {
        private readonly ITransferClient _transferClient;
        private readonly IBankAccountClient _accountClient;

        public Query(ITransferClient transferClient, IBankAccountClient accountClient)
        {
            _transferClient = transferClient;
            _accountClient = accountClient;
        }

        public async Task<List<TransferView>> GetTransactions()
            => await _transferClient.GetActiveTranfers();

        public async Task<TransferView> GetTransaction(Guid transactionId)
            => await _transferClient.GetTranferInfo(transactionId);

        public async Task<BankAccountDetailsView> GetAccountInfo(Guid accountId)
            => await _accountClient.GetDetailedInfo(accountId);
    }
}
