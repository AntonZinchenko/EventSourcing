using MediatR;
using System.Collections.Generic;
using Transfer.Contracts.Types;

namespace BankAccount.Application.Queries
{
    /// <summary>
    /// Запрос списка банковских переводов.
    /// </summary>
    public class GetTransactionsQuery : IRequest<List<TransferView>>
    {
    }
}
