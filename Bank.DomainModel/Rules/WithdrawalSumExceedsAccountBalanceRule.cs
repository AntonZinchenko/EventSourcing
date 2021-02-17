using SeedWorks.Validation;

namespace BankAccount.DomainModel.Rules
{
    /// <summary>
    /// Правило проверяющее, что сумма списания не превосходит текущий баланс.
    /// </summary>
    public class WithdrawalSumExceedsAccountBalanceRule : IBusinessRule
    {
        private readonly decimal _withdrawalSum;
        private readonly decimal _accountBalance;

        public WithdrawalSumExceedsAccountBalanceRule(decimal withdrawalSum, decimal accountBalance)
        {
            _withdrawalSum = withdrawalSum;
            _accountBalance = accountBalance;
        }

        public bool IsBroken() => _withdrawalSum > _accountBalance;

        public string Message => "Withdrawal sum exceeds current account balance.";
    }
}