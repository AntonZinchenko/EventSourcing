using BankAccount.Contracts.Requests;
using BankAccount.Contracts.Views;
using Gateway.Graphql.Types;
using Graphql.Graphql.Interfaces;
using HotChocolate.Subscriptions;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Contracts.Requests;
using Transfer.Contracts.Types;

namespace Gateway.Graphql.Models
{
    public class Mutation
    {
        private readonly ITransferClient _transferClient;
        private readonly IBankAccountClient _bankAccountClient;
        private readonly IEventDispatcher _eventDispatcher;

        public Mutation(
            ITransferClient transferClient,
            IBankAccountClient bankAccountClient,
            IEventDispatcher eventDispatcher)
        {
            _transferClient = transferClient;
            _bankAccountClient = bankAccountClient;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<BankAccountDetailsView> CreateBankAccountAsync(CreateBankAccountRequest request)
        {
            var account = await _bankAccountClient.Create(request.Owner);
            return await _bankAccountClient.GetDetailedInfo(account.Id);
        }

        public async Task<TransferView> ExecuteTransferAsync(TransferRequest request)
        {
            var id = await _transferClient.ExecuteTransfer(
                request.SourceAccountId,
                request.TargetAccountId,
                request.Sum);

            var transfer = await _transferClient.GetTranferInfo(id);

            await _eventDispatcher.SendAsync(
                    SubscriptionType.NewTransferChannel,
                    transfer,
                    CancellationToken.None)
                .ConfigureAwait(false);

            return transfer;
        }

        public async Task<BankAccountDetailsView> ProcessDepositeAsync(PerformDepositeRequestModel request)
        {
            await _bankAccountClient.ProcessDeposite(request.AccountId, request.Sum);
            return await _bankAccountClient.GetDetailedInfo(request.AccountId);
        }

        public async Task<BankAccountDetailsView> ProcessWithdrawalAsync(PerformWithdrawalRequestModel request)
        {
            await _bankAccountClient.ProcessWithdrawal(request.AccountId, request.Sum);
            return await _bankAccountClient.GetDetailedInfo(request.AccountId);
        }

        public async Task<BankAccountDetailsView> RenameOwnerAsync(ChangeOwnerRequestModel request)
        {
            await _bankAccountClient.RenameOwner(request.AccountId, request.NewOwner);
            return await _bankAccountClient.GetDetailedInfo(request.AccountId);
        }
    }
}
