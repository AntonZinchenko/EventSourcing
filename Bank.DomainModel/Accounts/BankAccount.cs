using Bank.DomainModel.Accounts.Events;
using Bank.DomainModel.Accounts.Rules;
using Newtonsoft.Json;
using SeedWorks;
using SeedWorks.Core.Aggregates;
using System;

namespace Bank.DomainModel.Accounts
{
    public class BankAccount: Aggregate
    {
        /// <summary>
        /// Имя владельца расчетного счета.
        /// </summary>
        public string Owner { get; private set; }

        /// <summary>
        /// Текущий баланс.
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Дата последней модификации данных.
        /// </summary>
        public DateTime LastModified { get; private set; }

        public BankAccount() { }

        public BankAccount(string owner, Guid correlationId)
        {
            CheckRules(new OwnerNameNotEmptyRule(owner));

            BankAccountCreated.Create(correlationId, owner)
                .Do(Enqueue)
                .Do(Apply);
        }

        [JsonConstructor]
        public BankAccount(Guid id, string owner, decimal balance, int version, DateTime lastModified) 
        {
            Id = id;
            Owner = owner;
            Balance = balance;
            Version = version;
            LastModified = lastModified;
        }

        /// <summary>
        /// Открыть расчетный счет.
        /// </summary>
        /// <param name="owner">Имя владельца.</param>
        public static BankAccount Create(string owner, Guid correlationId)
            => new BankAccount(owner, correlationId);

        /// <summary>
        /// Выполнить начисление депозитных процентов.
        /// </summary>
        /// <param name="sum">Сумма проводки.</param>
        public void PerformDeposite(decimal sum, Guid correlationId)
        {
            CheckRules(new DepositeSumIsPositiveRule(sum));

            DepositePerformed.Create(Id, correlationId, sum)
                .Do(Enqueue)
                .Do(Apply);
        }

        /// <summary>
        /// Переоформить счет на другого пользователя.
        /// </summary>
        /// <param name="newOwner">Имя нового владельца.</param>
        public void ChangeOwner(string newOwner, Guid correlationId)
        {
            CheckRules(new OwnerNameNotEmptyRule(newOwner));

            OwnerChanged.Create(Id, correlationId, newOwner)
                .Do(Enqueue)
                .Do(Apply);
        }

        /// <summary>
        /// Выполнить списание с расчетного счета.
        /// </summary>
        /// <param name="sum">Сумма списания.</param>
        public void PerformWithdrawal(decimal sum, Guid correlationId)
        {
            CheckRules(new WithdrawalSumExceedsAccountBalanceRule(sum, Balance));

            WithdrawalPerformed.Create(Id, correlationId, sum)
                .Do(Enqueue)
                .Do(Apply);
        }

        #region Обработчики событий агрегата

        public void Apply(BankAccountCreated @event)
        {
            Id = @event.AccountId;
            Owner = @event.Owner;
            Balance = 0;
            LastModified = @event.Created;
        }

        public void Apply(DepositePerformed @event)
        {
            Id = @event.AccountId;
            Balance += @event.Sum;
            LastModified = @event.Created;
        }

        public void Apply(OwnerChanged @event)
        {
            Id = @event.AccountId;
            Owner = @event.NewOwner;
            LastModified = @event.Created;
        }

        public void Apply(WithdrawalPerformed @event)
        {
            Id = @event.AccountId;
            Balance -= @event.Sum;
            LastModified = @event.Created;
        }

        #endregion
    }
}
