using shared.api.Enums;
using System.Numerics;

namespace shared.api.Dtos.Inputs;

public class CreateTransactionInputDto
{
    public decimal Amount { get; set; }
    public TransactionPriority Priority { get; set; } = TransactionPriority.HIGH;
    public string DestinationAddress { get; set; }
    public Guid FromAssetId { get;set; }
    public string PrivateKey { get; set; }
    public string SeedPhrase { get; set; }
}
