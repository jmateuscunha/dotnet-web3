using Dapper;
using model.api;
using Npgsql;
using shared.api.Channels;

public class EthereumCheckBalanceWorker : BackgroundService
{
    private readonly ILogger<EthereumCheckBalanceWorker> _logger;
    private readonly IChannel<dynamic> _channel;
    private readonly string _web3RPCProvider = Environment.GetEnvironmentVariable("SepoliaRPC");
    public EthereumCheckBalanceWorker(ILogger<EthereumCheckBalanceWorker> logger, 
                                 IChannel<dynamic> channel)
    {
        _logger=logger;
        _channel=channel;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckBalance();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task CheckBalance()
    {
        _logger.LogInformation("START => CheckBalance");
        using var connection = new NpgsqlConnection(Environment.GetEnvironmentVariable("PostgresConnectionStrings"));

        var assets = await connection.QueryAsync<Asset>(SqlCommands.GetEthSepoliaAssets);

        var web3 = new Nethereum.Web3.Web3(_web3RPCProvider);

        foreach (var asset in assets)
        {
            var balance = await web3.Eth.GetBalance.SendRequestAsync(asset.Address);

            _logger.LogInformation($"Id:{asset.Id} Address:{asset.Address} Balance:{asset.Balance}  Web3Balance:{balance}");

            if (asset.Balance != balance)
            {
                _logger.LogWarning($"Sending {asset.Id} to adjust balance");
                await _channel.AddItemInQueueAsync(new { asset.Id, Balance = balance.ToString() });
            }

        }
        _logger.LogInformation("END => CheckBalance");
    }
}