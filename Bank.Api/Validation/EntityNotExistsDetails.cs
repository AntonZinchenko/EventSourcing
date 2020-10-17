using Bank.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Validation
{
    public class EntityNotExistsDetails: ProblemDetails
    {
        public EntityNotExistsDetails(EntityNotFoundException exception)
        {
            Title = $"Can't find {exception.TypeName}.";
            Status = StatusCodes.Status404NotFound;
            Detail = exception.Message;
            Type = $"https://httpstatuses.com/{Status}";
        }
    }
}
