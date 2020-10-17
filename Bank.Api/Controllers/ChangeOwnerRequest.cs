namespace Bank.Api.Controllers
{
    public class ChangeOwnerRequest
    {
        /// <summary>
        /// Имя нового владельца.
        /// </summary>
        public string NewOwner { get; set; }
    }
}
