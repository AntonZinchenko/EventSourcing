using Graphql.Graphql.Interfaces;
using GreenDonut;
using HotChocolate.DataLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Contracts.Types;

namespace Graphql.Types.Loaders
{
    public class TransactionByIdDataLoader : BatchDataLoader<Guid, TransferView>
    {
        private readonly ITransferClient _transferClient;

        public TransactionByIdDataLoader(
            IBatchScheduler batchScheduler,
            ITransferClient transferClient)
            : base(batchScheduler)
        {
            _transferClient = transferClient ?? throw new ArgumentNullException(nameof(transferClient));
        }

        protected override async Task<IReadOnlyDictionary<Guid, TransferView>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            var transfers = keys.Select(id => _transferClient.GetTranferByIdAsync(id, cancellationToken)).ToArray();

            Task.WaitAll(transfers);

            return transfers.Select(a => a.Result).ToDictionary(a => a.Id, a => a);
        }
    }
}
