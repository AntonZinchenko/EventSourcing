using System;
using System.Threading.Tasks;

namespace Transfer.Application.Interfaces
{
    public interface IClient {}

    /// <summary>
    /// Клиент для отправки сообщений к службе BankAccount
    /// </summary>
    public interface IBankAccountClient 
    {
        /// <summary>
        /// Выполнить списание денежных средств. 
        /// </summary>
        Task ProcessWithdrawal(Guid accountId, decimal sum, Guid correlationId);

        /// <summary>
        /// Выполнить зачисление денежных средств.
        /// </summary>
        Task ProcessDeposite(Guid accountId, decimal sum, Guid correlationId);
    }
}
