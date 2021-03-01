using BankAccount.Contracts.Requests;
using BankAccount.Contracts.Views;
using Graphql.Graphql.Interfaces;
using MediatR;
using SeedWorks.HttpClients;
using System;
using System.Net.Http;
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
        public async Task<BankAccountShortInfoView> Create(string owner)
        {
            var model = new CreateBankAccountRequest { Owner = owner };

            return await this.PostAsync<CreateBankAccountRequest, BankAccountShortInfoView>(
                _httpClient,
                $"/api/commands",
                model,
                Guid.NewGuid());
        }

        /// <summary>
        /// Сменить имя владельца расчетного счета.
        /// </summary>
        public async Task RenameOwner(Guid accountId, string newOwner)
        {
            var model = new ChangeOwnerRequest { NewOwner = newOwner };

            await this.PatchAsync<ChangeOwnerRequest, Unit>(
                _httpClient,
                $"/api/commands/{accountId}/RenameOwner",
                model,
                Guid.NewGuid());
        }

        /// <summary>
        /// Выполнить зачисление денежных средств.
        /// </summary>
        public async Task ProcessDeposite(Guid accountId, decimal sum)
        {
            var model = new PerformDepositeRequest { Sum = sum };

            await this.PostAsync<PerformDepositeRequest, Unit>(
                _httpClient,
                $"/api/commands/{accountId}/performDeposite",
                model,
                Guid.NewGuid());
        }

        /// <summary>
        /// Выполнить списание денежных средств. 
        /// </summary>
        public async Task ProcessWithdrawal(Guid accountId, decimal sum)
        {
            var model = new PerformWithdrawalRequest { Sum = sum };

            await this.PostAsync<PerformWithdrawalRequest, Unit>(
                _httpClient,
                $"/api/commands/{accountId}/performWithdrawal",
                model,
                Guid.NewGuid());
        }

        /// <summary>
        /// Запросить краткую выписку по счету.
        /// </summary>
        public async Task<BankAccountShortInfoView> GetShortInfo(Guid accountId, int accountVersion = default)
            => await this.GetAsync<BankAccountShortInfoView>(
                _httpClient,
                $"/api/queries/{accountId}/{accountVersion}");

        /// <summary>
        /// Запросить полную выписку по счету.
        /// </summary>
        public async Task<BankAccountDetailsView> GetDetailedInfo(Guid accountId)
            => await this.GetAsync<BankAccountDetailsView>(
                _httpClient,
                $"/api/queries/{accountId}/Details");
        
    }
}
