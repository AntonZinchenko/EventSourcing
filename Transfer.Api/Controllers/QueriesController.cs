using BankAccount.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeedWorks;
using System;
using System.Threading.Tasks;

namespace Transfer.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class QueriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QueriesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Получить список активных транзакций.
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> Get()
            => (await _mediator.Send(new GetTransactionsQuery()))
               .Either(Ok, _ => (IActionResult)new NotFoundResult());

        /// <summary>
        /// Получить детализацию банковского перевода по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор банковского счета.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
            => (await _mediator.Send(new GetTransactionQuery(id)))
               .Either(Ok, _ => (IActionResult)new NotFoundResult());
    }
}
