using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace shared.api.Dtos.SmartContractOutputDto;

public class EventInputDto
{
    [Parameter("int256", "amount")]
    public BigInteger Amount { get; set; }

    [Parameter("address", "destinationAddress")]
    public string Address { get; set; }

}