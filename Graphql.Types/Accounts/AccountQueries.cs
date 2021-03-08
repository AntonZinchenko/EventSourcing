using BankAccount.Contracts.Views;
using Graphql.Types.Loaders;
using HotChocolate.Types;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Graphql.Types.Accounts
{
    [ExtendObjectType(Name = "Query")]
    public class AccountQueries
    {
        public async Task<BankAccountDetailsView> GetBankAccountById(
            Guid accountId,
            BankAccountByIdDataLoader dataLoader,
            CancellationToken cancellationToken)
            => await dataLoader.LoadAsync(accountId, cancellationToken);
    }
}
