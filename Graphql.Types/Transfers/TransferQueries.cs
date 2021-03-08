using Graphql.Graphql.Interfaces;
using Graphql.Types.Loaders;
using HotChocolate;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Contracts.Types;

namespace Graphql.Types.Transfers
{
    [ExtendObjectType(Name = "Query")]
    public class TransferQueries
    {
        public async Task<List<TransferView>> GetTransfers(
            [Service] ITransferClient transferClient,
            CancellationToken cancellationToken)
            => await transferClient.GetTranfersAsync(cancellationToken);

        public async Task<TransferView> GetTransferById(
            Guid transferId,
            TransactionByIdDataLoader dataLoader,
            CancellationToken cancellationToken)
            => await dataLoader.LoadAsync(transferId, cancellationToken);
    }
}
