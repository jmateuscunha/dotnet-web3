using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using shared.api.Errors;

namespace shared.api.FluentValidators;

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
    {        
        return new UnprocessableEntityObjectResult(new ApiErrorDTO
        {
            Errors = validationProblemDetails?.Errors.Select(c => new ErrorDTO { Code = "VALIDATION-001", Message = c.Value.FirstOrDefault() }),
            Timestamp = DateTime.UtcNow,
            TraceKey = context.HttpContext.Response.Headers["x-sample-tracekey"]
        });
    }
}
