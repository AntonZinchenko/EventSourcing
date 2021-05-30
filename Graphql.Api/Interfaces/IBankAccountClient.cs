using BankAccount.Contracts.Views;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Graphql.Graphql.Interfaces
{
    /// <summary>
    /// Клиент для отправки сообщений к службе BankAccount.
    /// </summary>
    public interface IBankAccountClient 
    {
        /// <summary>
        /// Открыть расчетный счет.
        /// </summary>
        Task<BankAccountShortInfoView> CreateAsync(string owner, CancellationToken cancellationToken);

        /// <summary>
        /// Сменить имя владельца расчетного счета.
        /// </summary>
        Task RenameOwnerAsync(Guid accountId, string newOwner, CancellationToken cancellationToken);

        /// <summary>
        /// Выполнить списание денежных средств. 
        /// </summary>
        Task ProcessWithdrawalAsync(Guid accountId, decimal sum, CancellationToken cancellationToken);

        /// <summary>
        /// Выполнить зачисление денежных средств.
        /// </summary>
        Task ProcessDepositeAsync(Guid accountId, decimal sum, CancellationToken cancellationToken);

        /// <summary>
        /// Запросить краткую выписку по счету.
        /// </summary>
        Task<BankAccountShortInfoView> GetBankAccountByIdAsync(Guid accountId, int accountVersion, CancellationToken cancellationToken);

        /// <summary>
        /// Запросить полную выписку по счету.
        /// </summary>
        Task<BankAccountDetailsView> GetBankAccountDetailsByIdAsync(Guid accountId, CancellationToken cancellationToken);
    }
}
