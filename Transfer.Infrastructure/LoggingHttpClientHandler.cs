using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Transfer.Infrastructure
{
    public class LoggingHttpClientHandler: HttpClientHandler
    {
        private readonly ILogger<LoggingHttpClientHandler> _logger;

        public LoggingHttpClientHandler(ILogger<LoggingHttpClientHandler> logger)
        {
            _logger = logger;
            Credentials = CredentialCache.DefaultCredentials;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var eventId = Guid.NewGuid();

            try
            {
                await LogHttpMessage("Request", eventId, request.ToString(), request.Content);

                var response = await base.SendAsync(request, cancellationToken);

                await LogHttpMessage("Response", eventId, response.ToString(), response.Content);

                return response;
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"Error[{eventId}] Can't send request");
                throw;
            }
        }

        private async Task LogHttpMessage(string type, Guid eventId, string commonData, HttpContent content)
        {
            _logger.LogInformation("{0}[{1}]:{2}{3}", type, eventId, Environment.NewLine, commonData);

            if (content != null)
            {
                var body = await content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(body))
                {
                    _logger.LogInformation("{0} body[{1}]:{2}{3}", type, eventId, Environment.NewLine, body);
                }
            }
        }
    }
}
