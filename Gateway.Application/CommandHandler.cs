using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Graphql.Application.Commands;
using Graphql.Application.Interfaces;

namespace Graphql.Application
{
    internal class CommandHandler :
        IRequestHandler<TransferBetweenAccountsCommand>
    {
        private readonly ITransferClient _transferClient;

        public CommandHandler(ITransferClient transferClient)
        {
            _transferClient = transferClient;
        }

        public async Task<Unit> Handle(TransferBetweenAccountsCommand request, CancellationToken cancellationToken)
            => await _transferClient.ExecuteTransfer(
                   request.SourceAccountId,
                   request.TargetAccountId,
                   request.Sum);
    }
}