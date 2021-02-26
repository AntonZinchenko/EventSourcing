using MediatR;
using System.Collections.Generic;
using Transfer.Contracts.Types;

namespace Graphql.Application.Queries
{
    /// <summary>
    /// Запрос списка банковских переводов.
    /// </summary>
    public class GetTransactionsQuery : IRequest<List<TransferView>>
    {
    }
}
