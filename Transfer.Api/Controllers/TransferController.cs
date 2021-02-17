using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeedWorks;
using System;
using System.Threading.Tasks;
using Transfer.Contracts.Events;
using Transfer.Contracts.Requests;

namespace Transfer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly IRequestClient<ISumTransferStarted> _transferClient;
        private readonly IExecutionContextAccessor _contextAccessor;

        public TransferController(
            IRequestClient<ISumTransferStarted> transferClient,
            IExecutionContextAccessor contextAccessor)
        {
            _transferClient = transferClient ?? throw new ArgumentNullException(nameof(IRequestClient<ISumTransferStarted>));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        /// <summary>
        /// Выполнить денежный перевод между счетами.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("execute")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> ExecuteTransferCommand([FromBody] TransferRequest request)
            => (await _transferClient.GetResponse<ISumTransferStarted>(new
            {
                request.SourceAccountId,
                request.TargetAccountId,
                request.Sum,
                _contextAccessor.CorrelationId
            })).PipeTo(_ => new AcceptedResult());
    }
}
