using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Validation
{
    public class FluentValidationExceptionProblemDetails : ProblemDetails
    {
        public FluentValidationExceptionProblemDetails(ValidationException exception)
        {
            Title = "Incorrect request";
            Status = StatusCodes.Status400BadRequest;
            Detail = exception.Message;
        }
    }
}
