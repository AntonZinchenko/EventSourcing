using HotChocolate.Types;
using Transfer.Contracts.Types;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Subscriptions;
using System.Threading;
using HotChocolate.Execution;

namespace Graphql.Types.Transfers
{
    [ExtendObjectType(Name = "Subscription")]
    public class TransferSubscriptions
    {
        public static string NewTransferChannel = "NewTransferRequests";

        [SubscribeAndResolve]
        public async Task<ISourceStream<TransferView>> NewTransferStarted([Service] ITopicEventReceiver eventReceiver, CancellationToken cancellationToken) 
            => await eventReceiver.SubscribeAsync<string, TransferView>(NewTransferChannel, cancellationToken);
    }
}