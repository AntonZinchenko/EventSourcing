using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankAccount.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeedWorks;

namespace BankAccount.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class QueriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QueriesController(IMediator mediator)
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
        /// <param name="version">Версия агрегата расчетного счета.</param>
        [HttpGet("{id}/{version}")]
        public async Task<IActionResult> GetShortInfo(Guid id, int version = default)
            => (await _mediator.Send(new GetBankAccountShortInfoQuery(id, version)))
               .Either(Ok, _ => (IActionResult)new NotFoundResult());

        /// <summary>
        /// Получить детализацию по расчетному счету.
        /// </summary>
        /// <param name="id">Идентификатор расчетного счета.</param>
        /// <returns></returns>
        [HttpGet("{id}/Details")]
        public async Task<IActionResult> GetAccountDetails(Guid id)
            => (await _mediator.Send(new GetBankAccountDetailsQuery(id)))
                .Either(Ok, _ => (IActionResult)new NotFoundResult());
    }
}
