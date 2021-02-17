using MediatR;

namespace BankAccount.Application.Commands
{
    /// <summary>
    /// Команда пересобирает материализованные представления.
    /// </summary>
    public class RebuildAccountsViewsCommand : IRequest<Unit>
    {
    }
}
