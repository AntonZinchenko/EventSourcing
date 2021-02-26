using BankAccount.Contracts.Requests;
using BankAccount.Contracts.Views;
using Graphql.Application.Interfaces;
using MediatR;
using SeedWorks.HttpClients;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Graphql.Infrastructure.HttpClients
{
    public class BankAccountClient : IClient, IBankAccountClient
    {
        private readonly HttpClient _httpClient;
        public BankAccountClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task ProcessDeposite(Guid accountId, decimal sum)
        {
            var model = new PerformDepositeRequest { Sum = sum };

            await this.PostAsync<PerformDepositeRequest, Unit>(
                _httpClient,
                $"/api/commands/{accountId}/performDeposite",
                model,
                Guid.NewGuid());
        }

        public async Task ProcessWithdrawal(Guid accountId, decimal sum)
        {
            var model = new PerformWithdrawalRequest { Sum = sum };

            await this.PostAsync<PerformWithdrawalRequest, Unit>(
                _httpClient,
                $"/api/commands/{accountId}/performWithdrawal",
                model,
                Guid.NewGuid());
        }

        public async Task<BankAccountShortInfoView> GetShortInfo(Guid accountId)
            => await this.GetAsync<BankAccountShortInfoView>(
                _httpClient,
                $"/api/queries/{accountId}/Info");

        public async Task<BankAccountDetailsView> GetDetailedInfo(Guid accountId)
            => await this.GetAsync<BankAccountDetailsView>(
                _httpClient,
                $"/api/queries/{accountId}/Details");
        
    }
}
