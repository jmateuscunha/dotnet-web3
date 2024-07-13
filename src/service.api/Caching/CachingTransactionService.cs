using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using service.api.Interfaces;
using shared.api.Dtos.Inputs;
using shared.api.Dtos.Ouputs;

namespace service.api.Caching;

public class CachingTransactionService : ITransactionService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CachingTransactionService> _logger;
    private readonly ITransactionService _transactionService;
    private readonly string key = "all_transactions";
    public CachingTransactionService(ITransactionService inner, 
                                     ILogger<CachingTransactionService> logger, 
                                     IMemoryCache memoryCache)
    {
        _logger=logger;
        _memoryCache=memoryCache;
        _transactionService=inner;

    }

    public async Task<object> CallSmartContract(CallSmartContractInputDto dto)
    => await _transactionService.CallSmartContract(dto);

    public async Task<CreateTransactionOutputDto> CreateTransaction(CreateTransactionInputDto dto)
    => await _transactionService.CreateTransaction(dto);

    public async Task<object> DeploySmartContract(DeploySmartContractInputDto dto)
    => await _transactionService.DeploySmartContract(dto);

    public async Task<IEnumerable<TransactionOutputDto>> GetAll()
    {
        _logger.LogTrace($"get all transactions");
        var transactions = _memoryCache.Get<IEnumerable<TransactionOutputDto>>(key);
        if (transactions is null)
        {
            transactions = await _transactionService.GetAll();
            if (transactions is not null)
            {
                _logger.LogTrace($"set transactions to cache");
                _memoryCache.Set(key, transactions, TimeSpan.FromMinutes(2));
            }

            return transactions;

        }

        return transactions;
    }

    public async Task<GasPriceSuggestionOutputDto> GetPriorityGasPrices(CancellationToken ct)
    => await _transactionService.GetPriorityGasPrices(ct);
}
