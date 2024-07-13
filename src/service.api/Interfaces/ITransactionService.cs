using shared.api.Dtos.Inputs;
using shared.api.Dtos.Ouputs;

namespace service.api.Interfaces;

public interface ITransactionService : ICachingTransactionService
{
    Task<CreateTransactionOutputDto> CreateTransaction(CreateTransactionInputDto dto);
    Task<object> DeploySmartContract(DeploySmartContractInputDto dto);
    Task<object> CallSmartContract(CallSmartContractInputDto dto);
    Task<GasPriceSuggestionOutputDto> GetPriorityGasPrices(CancellationToken ct);
    
}

public interface ICachingTransactionService
{
    Task<IEnumerable<TransactionOutputDto>> GetAll();
}
