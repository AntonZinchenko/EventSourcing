﻿using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace SeedWorks.HttpClients
{
    public static class ClientExtensions
    {
        public static Task<TResponse> PostAsync<TRequest, TResponse>(
            this IClient client,
            HttpClient httpClient,
            string url,
            TRequest model,
            Guid correlationId,
            CancellationToken cancellationToken = new CancellationToken()) 
            => client.SendAsync<TRequest, TResponse>(httpClient, url, HttpMethod.Post, model, correlationId, cancellationToken);

        public static Task<TResponse> PatchAsync<TRequest, TResponse>(
            this IClient client,
            HttpClient httpClient,
            string url,
            TRequest model,
            Guid correlationId,
            CancellationToken cancellationToken = new CancellationToken())
            => client.SendAsync<TRequest, TResponse>(httpClient, url, HttpMethod.Patch, model, correlationId, cancellationToken);

        public static async Task<TResponse> SendAsync<TRequest, TResponse>(
            this IClient client,
            HttpClient httpClient,
            string url,
            HttpMethod method,
            TRequest model,
            Guid correlationId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{httpClient.BaseAddress}{url}"),
                Method = method,
                Content = new ObjectContent<TRequest>(model, new JsonMediaTypeFormatter())
            };

            request.Headers.Add(CorrelationMiddleware.CorrelationHeaderKey, correlationId.ToString());

            using var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<TResponse>(cancellationToken);
        }

        public static async Task<TResponse> GetAsync<TResponse>(
            this IClient client,
            HttpClient httpClient,
            string url,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<TResponse>();
        }
    }
}
