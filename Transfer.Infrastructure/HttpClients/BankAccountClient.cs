using BankAccount.Contracts.Requests;
using MediatR;
using SeedWorks.HttpClients;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Transfer.Application.Interfaces;

namespace Transfer.Infrastructure.HttpClients
{
    public class BankAccountClient : IClient, IBankAccountClient
    {
        private readonly HttpClient _httpClient;
        public BankAccountClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task ProcessDeposite(Guid accountId, decimal sum, Guid correlationId)
        {
            var model = new PerformDepositeRequest { Sum = sum };

            await this.PostAsync<PerformDepositeRequest, Unit>(
                _httpClient,
                $"/api/commands/{accountId}/performDeposite",
                model,
                correlationId);
        }

        public async Task ProcessWithdrawal(Guid accountId, decimal sum, Guid correlationId)
        {
            var model = new PerformWithdrawalRequest { Sum = sum };

            await this.PostAsync<PerformWithdrawalRequest, Unit>(
                _httpClient,
                $"/api/commands/{accountId}/performWithdrawal",
                model,
                correlationId);
        }
    }
}
