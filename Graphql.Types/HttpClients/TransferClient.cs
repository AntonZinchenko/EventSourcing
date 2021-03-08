using SeedWorks.HttpClients;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Transfer.Contracts.Types;
using Transfer.Contracts.Requests;
using Graphql.Graphql.Interfaces;
using System.Threading;

namespace Graphql.Graphql.HttpClients
{
    public class TransferClient : IClient, ITransferClient
    {
        private readonly HttpClient _httpClient;
        public TransferClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TransferView>> GetTranfersAsync(CancellationToken cancellationToken)
            => await this.GetAsync<List<TransferView>>(
                _httpClient,
                "/api/queries",
                cancellationToken);

        public async Task<TransferView> GetTranferByIdAsync(Guid id, CancellationToken cancellationToken)
            => await this.GetAsync<TransferView>(
                _httpClient,
                $"/api/queries/{id}",
                cancellationToken);

        public async Task<Guid> ExecuteTransferAsync(Guid sourceAccountId, Guid targetAccountId, decimal sum, CancellationToken cancellationToken)
        {
            var model = new TransferRequest 
            {
                SourceAccountId = sourceAccountId, 
                TargetAccountId = targetAccountId, 
                Sum = sum 
            };

            return await this.PostAsync<TransferRequest, Guid>(
                _httpClient,
                "/api/commands/execute",
                model,
                Guid.NewGuid(),
                cancellationToken);
        }
    }
}
