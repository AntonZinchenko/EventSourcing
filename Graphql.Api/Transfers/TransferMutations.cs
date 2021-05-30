using Graphql.Graphql.Interfaces;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Contracts.Requests;
using Transfer.Contracts.Types;

namespace Graphql.Types.Transfers
{
    [ExtendObjectType(Name = "Mutation")]
    public class TransferMutations
    {
        private readonly ITransferClient _transferClient;

        public TransferMutations(ITransferClient transferClient)
        {
            _transferClient = transferClient;
        }

        public async Task<TransferView> ExecuteTransferAsync(
            TransferRequest request,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken)
        {
            var id = await _transferClient.ExecuteTransferAsync(
                request.SourceAccountId,
                request.TargetAccountId,
                request.Sum,
                cancellationToken);

            var transfer = await _transferClient.GetTranferByIdAsync(id, cancellationToken);
            await eventSender.SendAsync(
                    TransferSubscriptions.NewTransferChannel,
                    transfer,
                    CancellationToken.None)
                .ConfigureAwait(false);

            return transfer;
        }
    }
}
