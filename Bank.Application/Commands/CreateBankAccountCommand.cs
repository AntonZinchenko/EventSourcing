using FluentValidation;
using SeedWorks;
using System;

namespace BankAccount.Application.Commands
{
    /// <summary>
    /// Команда открытия расчетного счета.
    /// </summary>
    public class CreateBankAccountCommand : CorrelationByRequest<Guid>
    {
        public CreateBankAccountCommand(string owner, Guid correlationId)
            : base(correlationId)
        {
            Owner = owner;
        }

        /// <summary>
        /// Имя владельца.
        /// </summary>
        public string Owner { get; }
    }

    public class CreateBankAccountValidator : AbstractValidator<CreateBankAccountCommand>
    {
        public CreateBankAccountValidator()
        {
            RuleFor(request => request.Owner).NotEmpty();
        }
    }
}
