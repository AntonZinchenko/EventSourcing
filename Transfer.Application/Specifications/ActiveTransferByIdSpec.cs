using Ardalis.Specification;
using System;
using Transfer.Application.Orchestrators;

namespace Transfer.Application.Specifications
{
    public class ActiveTransferByIdSpec : Specification<TransferState>
    {
        public ActiveTransferByIdSpec(Guid id)
        {
            Query.AsNoTracking()
                .Where(t => t.CurrentState != "Final" && t.CorrelationId == id);
        }
    }
}
