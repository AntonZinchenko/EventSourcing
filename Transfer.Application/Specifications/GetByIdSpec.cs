using Ardalis.Specification;
using System;
using Transfer.Application.Orchestrators;

namespace Transfer.Application.Specifications
{
    public class GetByIdSpec : Specification<TransferState>
    {
        public GetByIdSpec(Guid id)
        {
            Query.AsNoTracking().Where(t => t.CorrelationId == id);
        }
    }
}
