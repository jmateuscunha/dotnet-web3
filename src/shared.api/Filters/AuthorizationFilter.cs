using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using shared.api.Dtos;
using shared.api.Exceptions;

namespace shared.api.Filters;

public class AuthorizationFilter : IAsyncAuthorizationFilter
{
    private readonly string[] _rolesNeeded;
    private readonly ApiConfiguration _configuration;

    public AuthorizationFilter(string[] rolesNeeded, ApiConfiguration configuration)
    {
        _rolesNeeded = rolesNeeded;
        _configuration=configuration;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = await Task.FromResult(context.HttpContext);

        if (context.HttpContext.User.Identity.IsAuthenticated == false)
            throw new DomainException("INTERNAL", "Invalid credentials.");
    }
}


public class SampleAuthorizationAttribute : TypeFilterAttribute
{
    public SampleAuthorizationAttribute(params string[] rolesNeeded) : base(typeof(AuthorizationFilter))
    {
        Arguments = new object[] { rolesNeeded };
    }
}