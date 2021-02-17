namespace BankAccount.Contracts.Requests
{
    public class ChangeOwnerRequest
    {
        /// <summary>
        /// Имя нового владельца.
        /// </summary>
        public string NewOwner { get; set; }
    }
}
