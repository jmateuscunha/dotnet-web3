using shared.api.Abstracts;
using shared.api.Errors;

namespace shared.api.Exceptions;

public sealed class AuthenticationException : BaseException
{
    public AuthenticationException(IEnumerable<ErrorDTO> errors)
        : base(errors)
    {
    }

    public AuthenticationException(ErrorDTO error)
        : base(error)
    {
    }

    public AuthenticationException(string errorCode, string errorMessage)
        : base(errorCode, errorMessage)
    {
    }
}
