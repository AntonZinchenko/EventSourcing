using MediatR;
using System;
using System.Collections.Generic;

namespace Bank.Application.Accounts.Queries
{
    /// <summary>
    /// Запрос списка доступных расчетных счетов.
    /// </summary>
    public class GetBankAccountsQuery : IRequest<Dictionary<Guid, string>>
    {
    }
}
