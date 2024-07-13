namespace shared.api.Dtos;

public class ApiConfiguration
{
    public string ApiIssuer { get; set; }
    public string ApiAudience { get; set; }
    public string ApiIssuerKey { get; set; }
    public string PostgresConnectionStrings { get; set; }
    public string SepoliaRPC { get;set; }
    public string EtherscanApiKey { get;set; }
    public string EtherscanUrl { get; set; }
    public string GrpcServiceUrl { get; set; }
}