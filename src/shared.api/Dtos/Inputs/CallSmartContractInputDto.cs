using System.ComponentModel.DataAnnotations;

namespace shared.api.Dtos.Inputs;

public class CallSmartContractInputDto
{
    [Required]
    public string Abi { get; set; }
    [Required]
    public string ContractAddress { get; set; }
    [Required]
    public Guid AssetId { get; set; }
    [Required]
    public string SeedPhrase { get; set; }
    public string FunctionName { get; set; }
    public object Parameters { get; set; }
}
