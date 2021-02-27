using Ardalis.Specification;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Transfer.Application.Orchestrators;

namespace Transfer.Application.Specifications
{
    public class ActiveTransferSpec : Specification<TransferState>
    {
        public ActiveTransferSpec()
        {
            Query.AsNoTracking()
                .Where(t => t.CurrentState != "Final");
        }
    }
}
