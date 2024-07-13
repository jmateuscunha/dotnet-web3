using shared.api.Dtos.Ouputs;

namespace communication.api.Interfaces;

public interface IEthereumClient
{
    Task<GasPriceSuggestionOutputDto> GetPriorityGasPrices(CancellationToken ct);
}
