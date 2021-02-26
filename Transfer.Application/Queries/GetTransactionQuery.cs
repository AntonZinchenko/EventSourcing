using FluentValidation;
using MediatR;
using System;
using Transfer.Contracts.Types;

namespace BankAccount.Application.Queries
{
    public class GetTransactionQuery : IRequest<TransferView>
    {
        public GetTransactionQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public class GetTransactionQueryValidator : AbstractValidator<GetTransactionQuery>
    {
        public GetTransactionQueryValidator()
        {
            RuleFor(request => request.Id).NotEmpty();
        }
    }
}
