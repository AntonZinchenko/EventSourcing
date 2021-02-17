using MediatR;
using System;
using System.Collections.Generic;

namespace BankAccount.Application.Queries
{
    /// <summary>
    /// Запрос списка доступных расчетных счетов.
    /// </summary>
    public class GetBankAccountsQuery : IRequest<Dictionary<Guid, string>>
    {
    }
}
