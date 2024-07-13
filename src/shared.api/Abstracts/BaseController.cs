using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using shared.api.Errors;
using shared.api.Validations.Interfaces;

namespace shared.api.Abstracts;

[ApiController]
[Authorize]
public abstract class BaseController : ControllerBase
{
    private readonly IDomainValidation _validationContext;

    protected BaseController(IDomainValidation validationContext)
    {
        _validationContext = validationContext;
    }

    protected IActionResult CustomResponse(object result = null)
    {
        if (HasErrors())
        {
            return UnprocessableEntity(new ApiErrorDTO()
            {
                Timestamp = DateTime.UtcNow,
                Errors = _validationContext.GetErrors().ToList().Select(x => new ErrorDTO() { Code = "SAMPLE-001", Message = x }),
            });
        }

        return Ok(result);
    }

    protected bool HasErrors() => _validationContext.HasErrors();

    protected void AddError(string errorMessage) => _validationContext.AddError(errorMessage);

    protected void ClearErrors() => _validationContext.Clear();
}