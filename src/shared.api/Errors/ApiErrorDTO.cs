namespace shared.api.Errors;

public class ApiErrorDTO
{
    public DateTime Timestamp { get; set; }
    public string TraceKey { get; set; }
    public IEnumerable<ErrorDTO> Errors { get; set; }

    public ApiErrorDTO() { }
    public ApiErrorDTO(DateTime timestamp, string tracekey)
    {
        Timestamp = timestamp;
        TraceKey = tracekey;
    }

    public void AddErrors(IEnumerable<ErrorDTO> errors) => Errors = errors;
    public void AddError(ErrorDTO error) => AddErrors(new List<ErrorDTO> { error }.AsEnumerable());
    public void AddError(string errorCode, string errorMessage) => AddError(new ErrorDTO(errorCode, errorMessage));

}

public class ErrorDTO
{
    public string Code { get; set; }
    public string Message { get; set; }
    public ErrorDTO() { }
    public ErrorDTO(string code, string message)
    {
        Code = code;
        Message = message;
    }
}