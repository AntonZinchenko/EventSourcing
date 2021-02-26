using System;
using System.Collections.Generic;
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
        /// Получить список активных транзакций.
        /// </summary>
        Task<List<TransferView>> GetActiveTranfers();

        /// <summary>
        /// Выполнить денежный перевод между счетами.
        /// </summary>
        Task<Guid> ExecuteTransfer(Guid sourceAccountId, Guid targetAccountId, decimal sum);

        Task<TransferView> GetTranferInfo(Guid id);
    }
}
