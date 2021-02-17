using System;
using System.Linq;
using System.Collections.Generic;

namespace BankAccount.MaterializedView.Views
{
    public class BankAccountDetailsView
    {
        public BankAccountDetailsView()
        {
            CashFlow = new List<CashFlowItem>();
        }

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
        public decimal Balance => CashFlow.Sum(i => i.Sum);

        /// <summary>
        /// Движение денежных средств.
        /// </summary>
        public List<CashFlowItem> CashFlow { get; }
    }

    public class CashFlowItem
    {
        public CashFlowItem(DateTime date, decimal sum)
        {
            Date = date;
            Sum = sum;
        }

        /// <summary>
        /// Дата проведения операции.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Сумма проводки.
        /// </summary>
        public decimal Sum { get; }
    }
}
