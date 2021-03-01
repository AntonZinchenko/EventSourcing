using BankAccount.Contracts.Views;
using System;
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
        Task<BankAccountShortInfoView> Create(string owner);

        /// <summary>
        /// Сменить имя владельца расчетного счета.
        /// </summary>
        Task RenameOwner(Guid accountId, string newOwner);

        /// <summary>
        /// Выполнить списание денежных средств. 
        /// </summary>
        Task ProcessWithdrawal(Guid accountId, decimal sum);

        /// <summary>
        /// Выполнить зачисление денежных средств.
        /// </summary>
        Task ProcessDeposite(Guid accountId, decimal sum);

        /// <summary>
        /// Запросить краткую выписку по счету.
        /// </summary>
        Task<BankAccountShortInfoView> GetShortInfo(Guid accountId, int accountVersion = default);

        /// <summary>
        /// Запросить полную выписку по счету.
        /// </summary>
        Task<BankAccountDetailsView> GetDetailedInfo(Guid accountId);
    }
}
