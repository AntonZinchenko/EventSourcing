using MediatR;
using System;

namespace BankAccount.Application.Commands
{
    public class CorrelationByRequest<T> : IRequest<T>
    {
        public CorrelationByRequest(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        /// <summary>
        /// Маркер корреляции.
        /// </summary>
        public Guid CorrelationId { get; }
    }
}
