using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace shared.api.Dtos.SmartContractOutputDto;

[FunctionOutput]
public class BalanceOutputDto : IFunctionOutputDTO
{
    [Parameter("int256", "", 1)]
    public virtual BigInteger Amount { get; set; }
}
