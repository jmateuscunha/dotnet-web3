using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using shared.api.Errors;
using shared.api.Exceptions;

namespace shared.api.Handlers;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    public ExceptionHandlerMiddleware(RequestDelegate next, 
                                            ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger=logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            await HandleException(httpContext, exception);
        }
    }

    private async Task HandleException(HttpContext context, Exception ex)
    {
        _logger.LogError(message: ex.Message);
        _logger.LogError(message: ex.InnerException?.Message);

        context.Response.ContentType = "application/json";

        var apiErrorDTO = new ApiErrorDTO(timestamp: DateTime.UtcNow, tracekey: Guid.NewGuid().ToString());

        switch (ex)
        {
            case AuthenticationException authenticationException:
                {
                    context.Response.StatusCode = 401;
                    apiErrorDTO.AddErrors(MappingErrors(authenticationException.Errors));
                    break;
                }
            case IntegrationException integrationException:
                {
                    context.Response.StatusCode = integrationException.HttpStatusCode;
                    apiErrorDTO.AddErrors(MappingErrors(integrationException.Errors));
                    break;
                }
            case DomainException domainException:
                {
                    context.Response.StatusCode = 422;
                    apiErrorDTO.AddErrors(MappingErrors(domainException.Errors));
                    break;
                }
            case SecurityTokenExpiredException:
                {
                    context.Response.StatusCode = 401;
                    apiErrorDTO.AddError(errorCode: "SAMPLE-002", errorMessage: "The token is expired.");
                    break;
                }
            default:
                {
                    context.Response.StatusCode = 500;
                    apiErrorDTO.AddError(errorCode: "SAMPLE-002", errorMessage: "Internal Server Error.");

                    break;
                }
        }

        await context.Response.WriteAsync(JsonConvert.SerializeObject(apiErrorDTO, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
    }

    private static IEnumerable<ErrorDTO> MappingErrors(IEnumerable<ErrorDTO> errors)
    {
        if (errors is null) return Enumerable.Empty<ErrorDTO>();

        return errors.Select(custodyError => new ErrorDTO(custodyError.Code, custodyError.Message));
    }
}