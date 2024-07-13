namespace shared.api.Dtos.Ouputs;

public class WalletOutputDto
{
    public Guid Id { get;set; }
    public string Name { get; set; }
    public List<AssetOutputDto> Assets { get; set; }
}