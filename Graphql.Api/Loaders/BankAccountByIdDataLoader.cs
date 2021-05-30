using BankAccount.Contracts.Views;
using Graphql.Graphql.Interfaces;
using GreenDonut;
using HotChocolate.DataLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Graphql.Types.Loaders
{
    public class BankAccountByIdDataLoader : BatchDataLoader<Guid, BankAccountDetailsView>
    {
        private readonly IBankAccountClient _bankAccountClient;

        public BankAccountByIdDataLoader(
            IBatchScheduler batchScheduler,
            IBankAccountClient bankAccountClient)
            : base(batchScheduler)
        {
            _bankAccountClient = bankAccountClient ?? throw new ArgumentNullException(nameof(bankAccountClient));
        }

        protected override async Task<IReadOnlyDictionary<Guid, BankAccountDetailsView>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            var accounts = keys.Select(id => _bankAccountClient.GetBankAccountDetailsByIdAsync(id, cancellationToken)).ToArray();

            Task.WaitAll(accounts);

            return accounts.Select(a => a.Result).ToDictionary(a => a.Id, a => a);
        }
    }
}
