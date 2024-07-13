namespace shared.api.Dtos.Ouputs;

public class AddAssetOutputDto
{
    public Guid Id { get;set; }
    public string Address { get; set; }
    public string PrivateKey { get; set; }
    public string PublicKey { get; set; }
    public string SeedPhrase { get; set; }
}