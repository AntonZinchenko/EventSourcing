using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Application.Interfaces;

namespace Transfer.Storage
{
    public class QueryRepository<T> : IQueryRepository<T> where T : class
    {
        protected readonly TransferDbContext DbContext;

        public QueryRepository(TransferDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default) 
            => await DbContext.Set<T>().ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default) 
            => await ApplySpecification(spec).ToListAsync(cancellationToken);

        public async Task<T> FindByAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
            => await ApplySpecification(spec).SingleOrDefaultAsync(cancellationToken);

        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec).CountAsync(cancellationToken);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var evaluator = new SpecificationEvaluator<T>();
            return evaluator.GetQuery(DbContext.Set<T>().AsQueryable(), spec);
        }
    }
}
