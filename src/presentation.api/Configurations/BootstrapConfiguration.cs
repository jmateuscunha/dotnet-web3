using communication.api;
using communication.api.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Nethereum.JsonRpc.Client;
using Polly;
using repository.api;
using repository.api.Interfaces;
using repository.api.Repositories;
using service.api;
using service.api.Caching;
using service.api.Interfaces;
using shared.api.Converters;
using shared.api.Dtos;
using shared.api.Extensions;
using shared.api.Filters;
using shared.api.FluentValidators;
using shared.api.Handlers;
using shared.api.Interceptors;
using shared.api.Validations;
using shared.api.Validations.Interfaces;
using shared.Protos;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace application.api.Configurations;

public static class BootstrapConfiguration
{
    public static ApiConfiguration GetEnvConfiguration(IConfiguration configuration)
    {
        return new ApiConfiguration()
        {
            ApiIssuer = configuration.GetValue<string>("ApiIssuer"),
            ApiIssuerKey = configuration.GetValue<string>("ApiIssuerKey"),
            ApiAudience = configuration.GetValue<string>("ApiAudience"),
            PostgresConnectionStrings = configuration.GetValue<string>("PostgresConnectionStrings"),
            SepoliaRPC = configuration.GetValue<string>("SepoliaRPC"),
            EtherscanApiKey = configuration.GetValue<string>("EtherscanApiKey"),
            EtherscanUrl = configuration.GetValue<string>("EtherscanUrl"),
            GrpcServiceUrl = configuration.GetValue<string>("GrpcServiceUrl"),
        };
    }

    public static IServiceCollection AddControllerAndMvcSettings(this IServiceCollection services)
    {
        services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddMvc(options =>
        {
            options.EnableEndpointRouting = false;
            options.Filters.Add(typeof(ValidateModelStateAttribute));
        }).AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.AllowTrailingCommas = true;
            opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            opts.JsonSerializerOptions.Converters.Add(new JsonConverters.BigIntegerConverter());
            opts.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;   
            opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        services.AddRouting(options => options.LowercaseUrls = true)
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<SampleAuthenticationOptions, AuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, null);

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITransactionService, TransactionService>();
        services.Decorate<ITransactionService, CachingTransactionService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<IAccountService, AccountService>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IBlockchainRepository, BlockchainRepository>();

        return services;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AddAssetInputValidator>();
        services.AddFluentValidationAutoValidation(opts =>
        {
            opts.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });

        return services;
    }

    public static IServiceCollection AddDomainValidation(this IServiceCollection services)
    => services.AddScoped<IDomainValidation, DomainValidation>();
    
    public static IServiceCollection AddHttpContextAndUserAccessor(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IUserAccessor, UserAccessor>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, ApiConfiguration apiconfiguration, IConfiguration configuration)
    {
        services.AddDbContext<SampleDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("PostgresConnectionStrings"), b => b.MigrationsAssembly("application.api")));

        return services;
    }
 
    public static IServiceCollection AddHttpClients(this IServiceCollection services, ApiConfiguration apiconfiguration)
    {
        services.AddTransient<CommunicationDelegatingHandler>();

        services.AddHttpClient<IEthereumClient, EthereumClient>(client =>
        {
            client.BaseAddress = new Uri(apiconfiguration.EtherscanUrl);
        }).AddHttpMessageHandler<CommunicationDelegatingHandler>()
          .AddPolicyHandler(PollyExtensions.WaitAndRetry())
          .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));


        return services;
    }

    public static IServiceCollection AddGrpcServices(this IServiceCollection services, ApiConfiguration apiconfiguration)
    {
        services.AddScoped<IEthereumGrpcClient, EthereumGrpcClient>();

        services.AddSingleton<GrpcServiceInterceptor>();

        services.AddGrpcClient<GetWeather.GetWeatherClient>(client =>
        {
            client.Address = new Uri(apiconfiguration.GrpcServiceUrl);
        }).AddInterceptor<GrpcServiceInterceptor>();

        return services;
    }
}
