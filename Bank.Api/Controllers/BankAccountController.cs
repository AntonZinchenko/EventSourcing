using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Application.Accounts.Commands;
using Bank.Application.Accounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeedWorks;

namespace Bank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Получить список доступных расчетных счетов.
        /// </summary>
        [HttpGet()]
        public async Task<Dictionary<Guid, string>> GetAll()
            => await _mediator.Send(new GetBankAccountsQuery());

        /// <summary>
        /// Получить краткую информацию по расчетному счету.
        /// </summary>
        /// <param name="id">Идентификатор расчетного счета.</param>
        [HttpGet("{id}/info")]
        public async Task<IActionResult> GetShortInfoAsync(Guid id)
            => (await _mediator.Send(new GetBankAccountShortInfoQuery(id)))
               .Either(Ok, _ => (IActionResult)new NotFoundResult());

        /// <summary>
        /// Получить детализацию по расчетному счету.
        /// </summary>
        /// <param name="id">Идентификатор расчетного счета.</param>
        /// <returns></returns>
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetAccountDetails(Guid id)
            => (await _mediator.Send(new GetBankAccountDetailsQuery(id)))
                .Either(Ok, _ => (IActionResult)new NotFoundResult());

        /// <summary>
        /// Открыть расчетный счет.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateBankAccountRequest request)
            => (await _mediator.Send(new CreateBankAccountCommand(request.Owner)))
                .PipeTo(accountId => Created("api/BankAccount/info", accountId));

        /// <summary>
        /// Переоформить счет на другого пользователя.
        /// </summary>
        /// <param name="id">Идентификатор расчетного счета.</param>
        /// <param name="request"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPatch("{id}/renameOwner")]
        public async Task<IActionResult> ChangeOwner(Guid id, [FromBody] ChangeOwnerRequest request)
            => (await _mediator.Send(new ChangeOwnerCommand(id, request.NewOwner)))
                .PipeTo(_ => new OkResult());

        /// <summary>
        /// Выполнить начисление депозитных процентов.
        /// </summary>
        /// <param name="id">Идентификатор расчетного счета.</param>
        /// <param name="request"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPatch("{id}/performDeposite")]
        public async Task<IActionResult> PerformDeposite(Guid id, [FromBody] PerformDepositeRequest request)
            => (await _mediator.Send(new PerformDepositeCommand(id, request.Sum)))
                .PipeTo(_ => new OkResult());

        /// <summary>
        /// Выполнить списание с расчетного счета.
        /// </summary>
        /// <param name="id">Идентификатор расчетного счета.</param>
        /// <param name="request"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPatch("{id}/performWithdrawal")]
        public async Task<IActionResult> PerformWithdrawal(Guid id, [FromBody] PerformWithdrawalRequest request)
            => (await _mediator.Send(new PerformWithdrawalCommand(id, request.Sum)))
                .PipeTo(_ => new OkResult());

        /// <summary>
        /// Пересобрать материализованные представления.
        /// </summary>
        [HttpPost("rebuildViews")]
        public async Task<IActionResult> RebuildViews()
            => (await _mediator.Send(new RebuildAccountsViewsCommand()))
                .PipeTo(_ => NoContent());
    }
}
