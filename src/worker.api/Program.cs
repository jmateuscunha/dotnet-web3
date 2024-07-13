
using shared.api.Channels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(typeof(IChannel<>), typeof(BlockchainChannel<>));
builder.Services.AddHostedService<EthereumCheckBalanceWorker>();
builder.Services.AddHostedService<ConsumeWorker>();
builder.Services.AddHostedService<EthereumConfirmTransactionWorker>();

builder.Services.Configure<HostOptions>(o =>
{
    o.ServicesStartConcurrently = true;
    o.ServicesStopConcurrently = true;
    o.BackgroundServiceExceptionBehavior =  BackgroundServiceExceptionBehavior.Ignore;    
});

var app = builder.Build();

app.Run();