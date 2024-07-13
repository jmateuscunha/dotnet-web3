using Dapper;
using Npgsql;
using shared.api.Channels;

public class ConsumeWorker : BackgroundService
{
    private readonly ILogger<ConsumeWorker> _logger;
    private readonly IChannel<dynamic> _channel;
    public ConsumeWorker(ILogger<ConsumeWorker> logger, 
                         IChannel<dynamic> channel)
    {
        _logger=logger;
        _channel=channel;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await WaitAndConsume(stoppingToken);
        }
    }

    private async Task WaitAndConsume(CancellationToken stoppingToken)
    {
        var item = await _channel.DequeueItemAsync(stoppingToken);

        if(item is not null)
        {
            _logger.LogInformation("START => WaitAndConsume");
            
            using var connection = new NpgsqlConnection(Environment.GetEnvironmentVariable("PostgresConnectionStrings"));

            var rowAffected = await connection.ExecuteAsync(SqlCommands.UpdateEthSepoliaAssetsBalance, new { id = item.Id, balance = item.Balance });
            _logger.LogInformation($"RowAffected: {rowAffected > 0}  Id:{item.Id}  Balance:{item.Balance}");

            _logger.LogInformation("END => WaitAndConsume");
        }
    }
}