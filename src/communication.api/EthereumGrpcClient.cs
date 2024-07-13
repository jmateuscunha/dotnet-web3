using shared.Protos;

namespace communication.api;

public interface IEthereumGrpcClient
{
    Task<Object> GetAlgo();
}

public class EthereumGrpcClient : IEthereumGrpcClient
{
    private readonly GetWeather.GetWeatherClient _grpcClient;
    public EthereumGrpcClient(GetWeather.GetWeatherClient grpcClient)
    {
        _grpcClient=grpcClient;
    }

    public async Task<Object> GetAlgo()
    {
        var response = await _grpcClient.GetAsync(new GetWeatherRequest { });

        return response;
    }
}
