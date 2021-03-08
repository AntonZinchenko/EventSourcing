using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Contracts.Types;

namespace Graphql.Graphql.Interfaces
{
    /// <summary>
    /// Клиент для отправки сообщений к службе Transfer.
    /// </summary>
    public interface ITransferClient
    {
        /// <summary>
        /// Получить список транзакций.
        /// </summary>
        Task<List<TransferView>> GetTranfersAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить детализацию транзакции по Id.
        /// </summary>
        Task<TransferView> GetTranferByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Выполнить денежный перевод между счетами.
        /// </summary>
        Task<Guid> ExecuteTransferAsync(Guid sourceAccountId, Guid targetAccountId, decimal sum, CancellationToken cancellationToken);
    }
}
