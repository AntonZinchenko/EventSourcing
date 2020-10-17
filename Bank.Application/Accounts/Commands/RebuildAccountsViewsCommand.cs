using MediatR;

namespace Bank.Application.Accounts.Commands
{
    /// <summary>
    /// Команда пересобирает материализованные представления.
    /// </summary>
    public class RebuildAccountsViewsCommand : IRequest<Unit>
    {
    }
}
