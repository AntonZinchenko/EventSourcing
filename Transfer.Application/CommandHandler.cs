using MassTransit;
using MediatR;
using SeedWorks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Application.Commands;
using Transfer.Contracts.Events;

namespace Transfer.Application
{
    internal class CommandHandler :
        IRequestHandler<TransferBetweenAccountsCommand>
    {
        private readonly IRequestClient<ISumTransferStarted> _transferClient;

        public CommandHandler(IRequestClient<ISumTransferStarted> transferClient)
        {
            _transferClient = transferClient ?? throw new ArgumentNullException(nameof(transferClient));
        }

        public async Task<Unit> Handle(TransferBetweenAccountsCommand request, CancellationToken cancellationToken)
            => (await _transferClient.GetResponse<ISumTransferStarted>(new
               {
                   request.SourceAccountId,
                   request.TargetAccountId,
                   request.Sum,
                   request.CorrelationId
               })).PipeTo(_ => Unit.Value);
    }
}