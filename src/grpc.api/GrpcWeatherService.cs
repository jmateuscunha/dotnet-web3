using Grpc.Core;
using shared.Protos;

public class GrpcWeatherService : GetWeather.GetWeatherBase
{
    private readonly string[] summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
    public GrpcWeatherService()
    {

    }

    public override Task<GetWeatherResponse> Get(GetWeatherRequest request, ServerCallContext context)
    {
        var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast()).ToArray();

        var teste = new GetWeatherResponse()
        {
            Temperature = forecast.FirstOrDefault().TemperatureF
        };

        return Task.FromResult(teste);
    }

    public class WeatherForecast
    {
        public int TemperatureF => 32 + (int)(Random.Shared.Next(-20, 55) / 0.5556);
    }
}
