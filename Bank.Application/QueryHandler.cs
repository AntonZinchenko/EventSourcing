using BankAccount.Application.Queries;
using BankAccount.MaterializedView.Views;
using Marten;
using MediatR;
using SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankAccount.Application
{
    internal class QueryHandler:
        IRequestHandler<GetBankAccountDetailsQuery, BankAccountDetailsView>,
        IRequestHandler<GetBankAccountShortInfoQuery, BankAccountShortInfoView>,
        IRequestHandler<GetBankAccountsQuery, Dictionary<Guid, string>>
    {
        private readonly IDocumentSession _session;

        public QueryHandler(IDocumentSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        /// <summary>
        /// Обработчик запроса списка доступных расчетных счетов.
        /// </summary>
        public Task<Dictionary<Guid, string>> Handle(GetBankAccountsQuery request, CancellationToken cancellationToken)
            => _session.Query<BankAccountShortInfoView>()
                .ToDictionary(k => k.Id, v => v.Owner)
                .PipeTo(data => Task.FromResult(data));

        /// <summary>
        /// Обработчик запроса краткой информацию по расчетному счету.
        /// </summary>
        public Task<BankAccountShortInfoView> Handle(GetBankAccountShortInfoQuery request, CancellationToken cancellationToken)
            => _session.LoadAsync<BankAccountShortInfoView>(request.AccountId, cancellationToken);

        /// <summary>
        /// Обработчик запроса детализации по расчетному счету.
        /// </summary>
        public Task<BankAccountDetailsView> Handle(GetBankAccountDetailsQuery request, CancellationToken cancellationToken)
            => _session.LoadAsync<BankAccountDetailsView>(request.AccountId, cancellationToken);
    }
}
