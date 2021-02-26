using HotChocolate.Types;
using Transfer.Contracts.Types;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Subscriptions;
using System.Threading;

namespace Gateway.Graphql.Types
{
    [ExtendObjectType(Name = "Subscription")]
    public class SubscriptionType
    {
        public static string NewTransferChannel = "NewTransferRequests";

        [Subscribe]
        public async Task<IAsyncEnumerable<TransferView>> OnNewTransfersAsync(
            [Service] IEventTopicObserver eventTopicObserver,
            CancellationToken cancellationToken)
            => await eventTopicObserver.SubscribeAsync<string, TransferView>(
                    NewTransferChannel, cancellationToken)
                .ConfigureAwait(false);
    }
}
