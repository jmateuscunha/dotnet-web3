namespace shared.api.Dtos.Inputs;

public class DeploySmartContractInputDto
{
    public string Abi { get; set; }
    public string Bytecode { get; set; }
    public Guid AssetId { get; set; }
    public string SeedPhrase { get; set; }
}