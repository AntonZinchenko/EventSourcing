using BankAccount.Contracts.Requests;
using BankAccount.Contracts.Views;
using Graphql.Graphql.Interfaces;
using Graphql.Types.Accounts.Types.Models;
using HotChocolate.Types;
using System.Threading;
using System.Threading.Tasks;

namespace Graphql.Types.Accounts
{
    [ExtendObjectType(Name = "Mutation")]
    public class AccountMutations
    {
        private readonly IBankAccountClient _bankAccountClient;

        public AccountMutations(IBankAccountClient bankAccountClient)
        {
            _bankAccountClient = bankAccountClient;
        }

        public async Task<BankAccountDetailsView> CreateBankAccountAsync(
            CreateBankAccountRequest request,
            CancellationToken cancellationToken)
        {
            var account = await _bankAccountClient.CreateAsync(request.Owner, cancellationToken);
            return await _bankAccountClient.GetBankAccountDetailsByIdAsync(account.Id, cancellationToken);
        }

        public async Task<BankAccountDetailsView> ProcessDepositeAsync(
            PerformDepositeRequestModel request,
            CancellationToken cancellationToken)
        {
            await _bankAccountClient.ProcessDepositeAsync(request.AccountId, request.Sum, cancellationToken);
            return await _bankAccountClient.GetBankAccountDetailsByIdAsync(request.AccountId, cancellationToken);
        }

        public async Task<BankAccountDetailsView> ProcessWithdrawalAsync(
            PerformWithdrawalRequestModel request,
            CancellationToken cancellationToken)
        {
            await _bankAccountClient.ProcessWithdrawalAsync(request.AccountId, request.Sum, cancellationToken);
            return await _bankAccountClient.GetBankAccountDetailsByIdAsync(request.AccountId, cancellationToken);
        }

        public async Task<BankAccountDetailsView> RenameOwnerAsync(
            ChangeOwnerRequestModel request,
            CancellationToken cancellationToken)
        {
            await _bankAccountClient.RenameOwnerAsync(request.AccountId, request.NewOwner, cancellationToken);
            return await _bankAccountClient.GetBankAccountDetailsByIdAsync(request.AccountId, cancellationToken);
        }
    }
}
