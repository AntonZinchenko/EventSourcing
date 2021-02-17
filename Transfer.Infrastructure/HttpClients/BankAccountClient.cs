using BankAccount.Contracts.Requests;
using MediatR;
using SeedWorks;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Application.Interfaces;

namespace Transfer.Infrastructure.HttpClients
{
    public class BankAccountClient : IBankAccountClient
    {
        private readonly HttpClient _httpClient;
        public BankAccountClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task ProcessDeposite(Guid accountId, decimal sum, Guid correlationId)
        {
            var model = new PerformDepositeRequest { Sum = sum };
            await PostAsync<PerformDepositeRequest, Unit>(_httpClient, model, correlationId, $"{accountId}/performDeposite");
        }

        public async Task ProcessWithdrawal(Guid accountId, decimal sum, Guid correlationId)
        {
            var model = new PerformWithdrawalRequest { Sum = sum };
            await PostAsync<PerformWithdrawalRequest, Unit>(_httpClient, model, correlationId, $"{accountId}/performWithdrawal");
        }

        private async Task<TResponse> PostAsync<TRequest, TResponse>(
            HttpClient httpClient,
            TRequest model,
            Guid correlationId,
            string url = "",
            CancellationToken cancellationToken = new CancellationToken())
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{httpClient.BaseAddress}{url}"),
                Method = HttpMethod.Post,
                Content = new ObjectContent<TRequest>(model, new JsonMediaTypeFormatter())
            };

            request.Headers.Add(CorrelationMiddleware.CorrelationHeaderKey, correlationId.ToString());

            using var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TResponse>(cancellationToken);
        }
    }
}
