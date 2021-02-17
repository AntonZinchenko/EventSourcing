﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankAccount.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeedWorks;

namespace BankAccount.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankAccountController(
            IMediator mediator)
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
    }
}
