using Gateway.Graphql.Types;
using Graphql.Graphql.Interfaces;
using HotChocolate.Subscriptions;
using SeedWorks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Contracts.Requests;
using Transfer.Contracts.Types;

namespace Gateway.Graphql.Models
{
    public class Mutation
    {
        private readonly ITransferClient _transferClient;
        private readonly IEventDispatcher _eventDispatcher;

        public Mutation(
            ITransferClient transferClient,
            IEventDispatcher eventDispatcher)
        {
            _transferClient = transferClient;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<TransferView> ExecuteTransferAsync(TransferRequest request)
        {
            /*
            var id = await _transferClient.ExecuteTransfer(
                request.SourceAccountId,
                request.TargetAccountId,
                request.Sum);
            */

            var id = request.SourceAccountId;

            var transfer = await _transferClient.GetTranferInfo(id);

            await _eventDispatcher.SendAsync(
                    SubscriptionType.NewTransferChannel,
                    transfer,
                    CancellationToken.None)
                .ConfigureAwait(false);

            return transfer;
        }
    }
}
