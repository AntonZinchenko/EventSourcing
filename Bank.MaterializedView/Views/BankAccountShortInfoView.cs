using System;

namespace BankAccount.MaterializedView.Views
{
    public class BankAccountShortInfoView
    {
        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя владельца.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Текущий баланс.
        /// </summary>
        public decimal Balance { get; set; }
    }
}
