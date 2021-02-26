using AutoMapper;
using AutoMapper.QueryableExtensions;
using BankAccount.Application.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SeedWorks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Contracts.Types;
using Transfer.Storage;

namespace Transfer.Application
{
    internal class QueryHandler:
        IRequestHandler<GetTransactionsQuery, List<TransferView>>,
        IRequestHandler<GetTransactionQuery, TransferView>
    {
        private readonly TransferDbContext _context;
        private readonly IMapper _mapper;

        public QueryHandler(
            IMapper mapper,
            TransferDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Обработчик запроса списка банковских переводов.
        /// </summary>
        public Task<List<TransferView>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            return _context.TransferStates
                .AsNoTracking()
                // TODO: move to specification
                .Where(t => t.CurrentState != "Final")
                .ProjectTo<TransferView>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public Task<TransferView> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            return _context.TransferStates
                .AsNoTracking()
                // TODO: move to specification
                .Where(t => t.CurrentState != "Final" && t.CorrelationId == request.Id)
                .ProjectTo<TransferView>(_mapper.ConfigurationProvider)
                .SingleOrDefault()
                .PipeTo(Task.FromResult);
        }
    }
}
