namespace BankAccount.Contracts.Requests
{
    public class PerformWithdrawalRequest
    {
        /// <summary>
        /// Сумма списания.
        /// </summary>
        public decimal Sum { get; set; }
    }
}
