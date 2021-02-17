namespace BankAccount.Contracts.Requests
{
    public class PerformDepositeRequest
    {
        /// <summary>
        /// Сумма проводки.
        /// </summary>
        public decimal Sum { get; set; }
    }
}
