using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeedWorks;
using System;
using System.Threading.Tasks;
using Transfer.Application.Commands;
using Transfer.Contracts.Requests;

namespace Transfer.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IExecutionContextAccessor _contextAccessor;

        public CommandsController(
            IMediator mediator,
            IExecutionContextAccessor contextAccessor)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        /// <summary>
        /// Выполнить денежный перевод между счетами.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Execute")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult ExecuteTransferCommand([FromBody] TransferRequest request)
            => _mediator.Send(new TransferBetweenAccountsCommand(request.SourceAccountId, request.TargetAccountId, request.Sum, _contextAccessor.CorrelationId))
                .PipeTo(_ => new AcceptedResult());
    }
}
