using Dapper;
using model.api;
using Newtonsoft.Json;
using Npgsql;

public class EthereumConfirmTransactionWorker : BackgroundService
{
    private readonly ILogger<EthereumConfirmTransactionWorker> _logger;

    private readonly string _web3RPCProvider = Environment.GetEnvironmentVariable("SepoliaRPC");
    public EthereumConfirmTransactionWorker(ILogger<EthereumConfirmTransactionWorker> logger)
    {
        _logger=logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ConfirmPendingTransactions();
            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
        }
    }

    private async Task ConfirmPendingTransactions()
    {
        _logger.LogInformation("START => ConfirmTransactions");
        using var connection = new NpgsqlConnection(Environment.GetEnvironmentVariable("PostgresConnectionStrings"));

        var transactions = await connection.QueryAsync<Transaction>(SqlCommands.GetPendingEthSepoliaTransactions);

        var web3 = new Nethereum.Web3.Web3(_web3RPCProvider);

        foreach (var transaction in transactions)
        {
            var transactionReceipt = await web3.TransactionReceiptPolling.PollForReceiptAsync(transaction.Hash);
            
            _logger.LogInformation($"Id:{transaction.Id} Hash:{transaction.Hash}");
            _logger.LogInformation($"Receipt: {JsonConvert.SerializeObject(transactionReceipt)}");

            var rowAffected = await connection.ExecuteAsync(SqlCommands.ConfirmEthSepoliaTransaction, new { id = transaction.Id, blocknumber = transactionReceipt.BlockNumber.ToString() });
            _logger.LogInformation($"RowAffected: {rowAffected > 0}  Id:{transaction.Id}");
        }

        _logger.LogInformation("END => ConfirmTransactions");
    }
}