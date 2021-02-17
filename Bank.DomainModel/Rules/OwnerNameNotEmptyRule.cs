using SeedWorks.Validation;

namespace BankAccount.DomainModel.Rules
{
    /// <summary>
    /// Правило проверяющее, что имя владельца заполнено данными.
    /// </summary>
    public class OwnerNameNotEmptyRule : IBusinessRule
    {
        private readonly string _owner;

        public OwnerNameNotEmptyRule(string owner)
        {
            _owner = owner;
        }

        public bool IsBroken() => string.IsNullOrEmpty(_owner);

        public string Message => "Bank Account owner is empty.";
    }
}
