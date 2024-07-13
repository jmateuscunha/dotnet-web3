using shared.api.Errors;

namespace shared.api.Abstracts;

public abstract class BaseException : Exception
{
    public readonly IEnumerable<ErrorDTO> Errors;

    protected BaseException(IEnumerable<ErrorDTO> errors) => Errors = errors;

    protected BaseException(ErrorDTO error)
        : this(new List<ErrorDTO>() { error }.AsEnumerable())
    {
    }

    protected BaseException(string errorCode, string errorMessage)
        : this(new ErrorDTO(code: errorCode, message: errorMessage))
    {
    }
}
