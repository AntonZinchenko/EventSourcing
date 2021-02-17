using SeedWorks.Validation;

namespace BankAccount.DomainModel.Rules
{
    /// <summary>
    /// Правило проверяющее, что сумма начисления депозитных процентов положительная.
    /// </summary>
    public class DepositeSumIsPositiveRule : IBusinessRule
    {
        private readonly decimal _sum;

        public DepositeSumIsPositiveRule(decimal sum)
        {
            _sum = sum;
        }

        public bool IsBroken() => _sum <= 0;

        public string Message => "Deposite sum is incorrect.";
    }
}
