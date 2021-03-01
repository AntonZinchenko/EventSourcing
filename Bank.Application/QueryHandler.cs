using AutoMapper;
using BankAccount.Application.Queries;
using BankAccount.Contracts.Views;
using Marten;
using MediatR;
using SeedWorks;
using SeedWorks.Core.Storage;
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
        private readonly IMapper _mapper;
        private readonly IRepository<DomainModel.BankAccount> _repository;

        public QueryHandler(IDocumentSession session,
            IMapper mapper,
            IRepository<DomainModel.BankAccount> repository)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
        public async Task<BankAccountShortInfoView> Handle(GetBankAccountShortInfoQuery request, CancellationToken cancellationToken)
        {
            if (request.AccountVersion == default)
            {
                return await _session.LoadAsync<BankAccountShortInfoView>(request.AccountId, cancellationToken);
            }

            return (await _repository.Find(request.AccountId, request.AccountVersion, cancellationToken))
                .PipeTo(agg => _mapper.Map<BankAccountShortInfoView>(agg));
        }

        /// <summary>
        /// Обработчик запроса детализации по расчетному счету.
        /// </summary>
        public Task<BankAccountDetailsView> Handle(GetBankAccountDetailsQuery request, CancellationToken cancellationToken)
            => _session.LoadAsync<BankAccountDetailsView>(request.AccountId, cancellationToken);
    }
}
