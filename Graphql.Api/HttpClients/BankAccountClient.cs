using BankAccount.Contracts.Requests;
using BankAccount.Contracts.Views;
using Graphql.Graphql.Interfaces;
using MediatR;
using SeedWorks.HttpClients;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Graphql.Graphql.HttpClients
{
    public class BankAccountClient : IClient, IBankAccountClient
    {
        private readonly HttpClient _httpClient;
        public BankAccountClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Открыть расчетный счет.
        /// </summary>
        public async Task<BankAccountShortInfoView> CreateAsync(string owner, CancellationToken cancellationToken)
        {
            var model = new CreateBankAccountRequest { Owner = owner };

            return await this.PostAsync<CreateBankAccountRequest, BankAccountShortInfoView>(
                _httpClient,
                $"/api/commands",
                model,
                Guid.NewGuid(),
                cancellationToken);
        }

        /// <summary>
        /// Сменить имя владельца расчетного счета.
        /// </summary>
        public async Task RenameOwnerAsync(Guid accountId, string newOwner, CancellationToken cancellationToken)
        {
            var model = new ChangeOwnerRequest { NewOwner = newOwner };

            await this.PatchAsync<ChangeOwnerRequest, Unit>(
                _httpClient,
                $"/api/commands/{accountId}/RenameOwner",
                model,
                Guid.NewGuid(),
                cancellationToken);
        }

        /// <summary>
        /// Выполнить зачисление денежных средств.
        /// </summary>
        public async Task ProcessDepositeAsync(Guid accountId, decimal sum, CancellationToken cancellationToken)
        {
            var model = new PerformDepositeRequest { Sum = sum };

            await this.PostAsync<PerformDepositeRequest, Unit>(
                _httpClient,
                $"/api/commands/{accountId}/performDeposite",
                model,
                Guid.NewGuid(),
                cancellationToken);
        }

        /// <summary>
        /// Выполнить списание денежных средств. 
        /// </summary>
        public async Task ProcessWithdrawalAsync(Guid accountId, decimal sum, CancellationToken cancellationToken)
        {
            var model = new PerformWithdrawalRequest { Sum = sum };

            await this.PostAsync<PerformWithdrawalRequest, Unit>(
                _httpClient,
                $"/api/commands/{accountId}/performWithdrawal",
                model,
                Guid.NewGuid(),
                cancellationToken);
        }

        /// <summary>
        /// Запросить краткую выписку по счету.
        /// </summary>
        public async Task<BankAccountShortInfoView> GetBankAccountByIdAsync(Guid accountId, int accountVersion, CancellationToken cancellationToken)
            => await this.GetAsync<BankAccountShortInfoView>(
                _httpClient,
                $"/api/queries/{accountId}/{accountVersion}",
                cancellationToken);

        /// <summary>
        /// Запросить полную выписку по счету.
        /// </summary>
        public async Task<BankAccountDetailsView> GetBankAccountDetailsByIdAsync(Guid accountId, CancellationToken cancellationToken)
            => await this.GetAsync<BankAccountDetailsView>(
                _httpClient,
                $"/api/queries/{accountId}/Details",
                cancellationToken);
        
    }
}
