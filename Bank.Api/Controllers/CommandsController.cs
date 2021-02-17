using System;
using System.Threading.Tasks;
using BankAccount.Application.Commands;
using BankAccount.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeedWorks;

namespace BankAccount.Api.Controllers
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
            _mediator = mediator;
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        /// <summary>
        /// Открыть расчетный счет.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateBankAccountRequest request)
            => (await _mediator.Send(new CreateBankAccountCommand(request.Owner, _contextAccessor.CorrelationId)))
                .PipeTo(accountId => Created("api/Queries/Info", accountId));

        /// <summary>
        /// Переоформить счет на другого пользователя.
        /// </summary>
        /// <param name="id">Идентификатор расчетного счета.</param>
        /// <param name="request"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPatch("{id}/RenameOwner")]
        public async Task<IActionResult> ChangeOwner(Guid id, [FromBody] ChangeOwnerRequest request)
            => (await _mediator.Send(new ChangeOwnerCommand(id, request.NewOwner, _contextAccessor.CorrelationId)))
                .PipeTo(_ => new OkResult());

        /// <summary>
        /// Выполнить начисление депозитных процентов.
        /// </summary>
        /// <param name="id">Идентификатор расчетного счета.</param>
        /// <param name="request"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("{id}/PerformDeposite")]
        public async Task<IActionResult> PerformDeposite(Guid id, [FromBody] PerformDepositeRequest request)
            => (await _mediator.Send(new PerformDepositeCommand(id, request.Sum, _contextAccessor.CorrelationId)))
                .PipeTo(_ => new AcceptedResult());

        /// <summary>
        /// Выполнить списание с расчетного счета.
        /// </summary>
        /// <param name="id">Идентификатор расчетного счета.</param>
        /// <param name="request"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("{id}/PerformWithdrawal")]
        public async Task<IActionResult> PerformWithdrawal(Guid id, [FromBody] PerformWithdrawalRequest request)
            => (await _mediator.Send(new PerformWithdrawalCommand(id, request.Sum, _contextAccessor.CorrelationId)))
                .PipeTo(_ => new AcceptedResult());

        /// <summary>
        /// Пересобрать материализованные представления.
        /// </summary>
        [HttpPost("RebuildViews")]
        public async Task<IActionResult> RebuildViews()
            => (await _mediator.Send(new RebuildAccountsViewsCommand()))
                .PipeTo(_ => NoContent());
    }
}
