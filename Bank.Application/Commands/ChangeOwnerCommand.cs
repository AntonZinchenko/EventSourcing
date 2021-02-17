using FluentValidation;
using MediatR;
using SeedWorks;
using System;

namespace BankAccount.Application.Commands
{
    /// <summary>
    /// Команда переоформления счета на другого пользователя.
    /// </summary>
    public class ChangeOwnerCommand: CorrelationByRequest<Unit>
    {
        public ChangeOwnerCommand(Guid accountId, string newOwner, Guid correlationId)
            : base(correlationId)
        {
            AccountId = accountId;
            NewOwner = newOwner;
        }

        /// <summary>
        /// Идентификатор расчетного счета.
        /// </summary>
        public Guid AccountId { get; }

        /// <summary>
        /// Имя нового владельца.
        /// </summary>
        public string NewOwner { get; }
    }

    public class ChangeOwnerValidator: AbstractValidator<ChangeOwnerCommand>
    {
        public ChangeOwnerValidator()
        {
            RuleFor(request => request.AccountId).NotEmpty();
            RuleFor(request => request.NewOwner).NotEmpty();
        }
    }
}
