using shared.api.Errors;
using shared.api.Validations.Interfaces;
using System.Text;
using System.Text.Json;

namespace shared.api.Abstracts;

public abstract class BaseClient
{
    protected readonly HttpClient _httpClient;
    protected readonly IDomainValidation _validationContext;
    protected virtual string IntegrationAPI { get; }

    protected BaseClient(HttpClient httpClient,
                         IDomainValidation validationContext)
    {
        _httpClient = httpClient;
        _validationContext = validationContext;
    }

    protected async Task<TOutputData> HandleResponse<TOutputData>(HttpResponseMessage httpResponseMessage)
    {
        await HandleCommonErrors(httpResponseMessage);

        return await ParseResponse<TOutputData>(httpResponseMessage);
    }

    protected async Task HandleResponse(HttpResponseMessage httpResponseMessage)
    {
        await HandleCommonErrors(httpResponseMessage);
    }

    private async Task HandleCommonErrors(HttpResponseMessage httpResponseMessage)
    {
        int errorCode = (int)httpResponseMessage.StatusCode;

        if (errorCode >= 400 || errorCode == 0)
        {
            var errorResponse = await ParseError(httpResponseMessage);

            //throw new Exception(errorCode, errorResponse.Errors);
            throw new Exception();
        }
    }

    private static async Task<ApiErrorDTO> ParseError(HttpResponseMessage httpResponseMessage)
    {
        var errorResponse = await httpResponseMessage.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<ApiErrorDTO>(errorResponse);
    }


    private static async Task<TOutputData> ParseResponse<TOutputData>(HttpResponseMessage httpResponseMessage)
    {
        httpResponseMessage.EnsureSuccessStatusCode();

        var outputData = await JsonSerializer.DeserializeAsync<TOutputData>(await httpResponseMessage.Content.ReadAsStreamAsync());

        return outputData;
    }

    protected static StringContent ParseDataInJsonRequest<TInputData>(TInputData data)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(data, new JsonSerializerOptions(JsonSerializerDefaults.Web)),
            Encoding.UTF8,
            "application/json");

        return httpContent;
    }
}