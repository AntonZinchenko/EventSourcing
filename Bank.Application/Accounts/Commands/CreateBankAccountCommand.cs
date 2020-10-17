using FluentValidation;
using MediatR;
using System;

namespace Bank.Application.Accounts.Commands
{
    /// <summary>
    /// Команда открытия расчетного счета.
    /// </summary>
    public class CreateBankAccountCommand : IRequest<Guid>
    {
        public CreateBankAccountCommand(string owner)
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
