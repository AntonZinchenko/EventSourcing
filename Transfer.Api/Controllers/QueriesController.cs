using Microsoft.AspNetCore.Mvc;
using SeedWorks;
using System;

namespace Transfer.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class QueriesController : ControllerBase
    {
        private readonly IExecutionContextAccessor _contextAccessor;

        public QueriesController(
            IExecutionContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }
    }
}
