using shared.api.Abstracts;
using shared.api.Errors;

namespace shared.api.Exceptions;

public sealed class DomainException : BaseException
{
    public DomainException(IEnumerable<ErrorDTO> errors)
    : base(errors)
    {
    }

    public DomainException(ErrorDTO error)
        : base(error)
    {
    }

    public DomainException(string errorCode, string errorMessage)
        : base(errorCode, errorMessage)
    {
    }
}