using AutoMapper;
using BankAccount.Application.Queries;
using MediatR;
using SeedWorks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Application.Interfaces;
using Transfer.Application.Orchestrators;
using Transfer.Application.Specifications;
using Transfer.Contracts.Types;

namespace Transfer.Application
{
    internal class QueryHandler:
        IRequestHandler<GetTransactionsQuery, List<TransferView>>,
        IRequestHandler<GetTransactionQuery, TransferView>
    {
        private readonly IQueryRepository<TransferState> _queryRepository;
        private readonly IMapper _mapper;

        public QueryHandler(
            IQueryRepository<TransferState> queryRepository,
            IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Обработчик запроса списка банковских переводов.
        /// </summary>
        public async Task<List<TransferView>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
            => (await _queryRepository.ListAllAsync(cancellationToken))
                .PipeTo(_mapper.Map<List<TransferView>>);

        /// <summary>
        /// Обработчик запроса детализации банковского перевода по идентификатору.
        /// </summary>
        public async Task<TransferView> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
            => (await _queryRepository.FindByAsync(new GetByIdSpec(request.Id), cancellationToken))
                .PipeTo(_mapper.Map<TransferView>);
    }
}
