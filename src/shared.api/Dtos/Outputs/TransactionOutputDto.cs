using System.Numerics;

namespace shared.api.Dtos.Ouputs;

public class TransactionOutputDto
{
    public Guid Id { get; set; }
    public string Hash { get; set; }
    public BigInteger AmountRequested { get; set; }
    public BigInteger Nonce { get; set; }
}