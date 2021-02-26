//using HotChocolate.Subscriptions;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using Transfer.Contracts.Types;

//namespace Gateway.Graphql.Models
//{
//    public class Subscription
//    {
        

//        private readonly IEventTopicObserver _eventTopicObserver;
//        public Subscription(IEventTopicObserver eventTopicObserver)
//        {
//            _eventTopicObserver = eventTopicObserver;
//        }

//        public async Task<IAsyncEnumerable<TransferView>> OnNewTransfers() 
//            => await _eventTopicObserver.SubscribeAsync<string, TransferView>(
//                    NewTransferChannel,
//                    CancellationToken.None)
//                .ConfigureAwait(false);
//    }
//}
