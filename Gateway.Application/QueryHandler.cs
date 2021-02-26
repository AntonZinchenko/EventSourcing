using BankAccount.Contracts.Views;
using Graphql.Application.Interfaces;
using Graphql.Application.Queries;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Contracts.Types;

namespace Graphql.Application
{
    internal class QueryHandler:
        IRequestHandler<GetTransactionsQuery, List<TransferView>>,
        IRequestHandler<GetShortAccountIntoQuery, BankAccountShortInfoView>,
        IRequestHandler<GetDetailedAccountInfoQuery, BankAccountDetailsView>
    {
        private readonly IBankAccountClient _accountClient;
        private readonly ITransferClient _transferClient;

        public QueryHandler(
            IBankAccountClient accountClient,
            ITransferClient transferClient)
        {
            _accountClient = accountClient;
            _transferClient = transferClient;
        }

        /// <summary>
        /// Обработчик запроса списка банковских переводов.
        /// </summary>
        public Task<List<TransferView>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
            => _transferClient.GetActiveTranfers();

        /// <summary>
        /// Обработчик запроса краткой выписки банковского счета.
        /// </summary>
        public Task<BankAccountShortInfoView> Handle(GetShortAccountIntoQuery request, CancellationToken cancellationToken)
            => _accountClient.GetShortInfo(request.AccountId);

        /// <summary>
        /// Обработчик запроса полной выписки банковского счета.
        /// </summary>
        public Task<BankAccountDetailsView> Handle(GetDetailedAccountInfoQuery request, CancellationToken cancellationToken)
            => _accountClient.GetDetailedInfo(request.AccountId);
    }
}
