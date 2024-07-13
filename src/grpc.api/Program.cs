using Microsoft.AspNetCore.Authentication.JwtBearer;
using shared.api.Dtos;
using shared.api.Handlers;

var builder = WebApplication.CreateBuilder(args);

var apiconfiguration =  new ApiConfiguration()
    {
        ApiIssuer = Environment.GetEnvironmentVariable("ApiIssuer"),
        ApiIssuerKey = Environment.GetEnvironmentVariable("ApiIssuerKey"),
        ApiAudience = Environment.GetEnvironmentVariable("ApiAudience"),
    };

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddScheme<SampleAuthenticationOptions, AuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, null);

builder.Services.AddSingleton(apiconfiguration);

builder.Services.AddGrpc();


var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
//app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GrpcWeatherService>();
});

app.Run();