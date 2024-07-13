using shared.api.Abstracts;
using shared.api.Errors;

namespace shared.api.Exceptions;

public sealed class IntegrationException : BaseException
{
    public int HttpStatusCode { get; }
    public IntegrationException(IEnumerable<ErrorDTO> errors, int httpStatusCode)
        : base(errors)
    {
        HttpStatusCode = httpStatusCode;
    }

    public IntegrationException(ErrorDTO error, int httpStatusCode)
        : base(error)
    {
        HttpStatusCode = httpStatusCode;
    }

    public IntegrationException(string errorCode, string errorMessage, int httpStatusCode)
        : base(errorCode, errorMessage)
    {
        HttpStatusCode = httpStatusCode;
    }
}
