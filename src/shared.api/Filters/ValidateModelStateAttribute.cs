using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using shared.api.Errors;

namespace shared.api.Filters;

public class ValidateModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.SelectMany(c => c.Value.Errors)
                                           .Select(c => c.ErrorMessage);

            context.Result = new UnprocessableEntityObjectResult(new ApiErrorDTO
            {
                Errors = errors.Select(c => new ErrorDTO { Code = "SAMPLE-001", Message = c }),
                Timestamp = DateTime.UtcNow,
                TraceKey = context.HttpContext.Response.Headers["x-sample-tracekey"]
            });
        }
    }
}