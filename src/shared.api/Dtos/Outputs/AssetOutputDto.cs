using System.Numerics;

namespace shared.api.Dtos.Ouputs;

public class AssetOutputDto
{
    public Guid Id { get; set; }
    public string Address { get; set; }
    public BigInteger Balance { get; set; }
}