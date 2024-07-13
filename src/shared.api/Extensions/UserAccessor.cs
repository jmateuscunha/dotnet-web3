using Microsoft.AspNetCore.Http;

namespace shared.api.Extensions;

public interface IUserAccessor
{
    int GetAccountId();
}

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor=httpContextAccessor ??  throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public int GetAccountId()
    {
        var accountId = _httpContextAccessor.HttpContext.User.GetAccountId();
        return int.Parse(accountId);
    }
}
