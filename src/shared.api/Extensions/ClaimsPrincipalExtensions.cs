using System.Security.Claims;

namespace shared.api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetAccountId(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal is null)        
            throw new ArgumentException(nameof(claimsPrincipal));
        
        var claim = claimsPrincipal.FindFirst("accountId");
        return claim?.Value;
    }
}