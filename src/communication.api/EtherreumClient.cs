using communication.api.Interfaces;
using Grpc.Core;
using shared.api.Abstracts;
using shared.api.Dtos;
using shared.api.Dtos.Ouputs;
using shared.api.Exceptions;
using shared.api.Validations.Interfaces;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace communication.api;

public class EthereumClient : BaseClient, IEthereumClient
{
    private readonly ApiConfiguration _configuration;
    public EthereumClient(HttpClient httpClient,
                          IDomainValidation validationContext,
                          ApiConfiguration configuration) : base(httpClient, validationContext)
    {
        _configuration=configuration;
    }

    public async Task<GasPriceSuggestionOutputDto> GetPriorityGasPrices(CancellationToken ct)
    {
        var response = await _httpClient.GetAsync($"/api?module=gastracker&action=gasoracle&apikey={_configuration.EtherscanApiKey}");

        if(response.StatusCode is not HttpStatusCode.OK) throw new IntegrationException("INTEGRATION-001", "Failed to get gas price suggestions.", (int)response.StatusCode);

        return await response.Content.ReadFromJsonAsync<GasPriceSuggestionOutputDto>(ct);
    }
}
