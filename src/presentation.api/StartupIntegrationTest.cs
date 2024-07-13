using application.api.Configurations;
using Microsoft.EntityFrameworkCore;
using repository.api;
using shared.api.Converters;
using shared.api.Filters;
using shared.api.Handlers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace application.api.startup;

public class StartupIntegrationTest
{
    private IWebHostEnvironment Env { get; set; }
    public IConfiguration Configuration { get; }

    public StartupIntegrationTest(IWebHostEnvironment hostEnvironment)
    {
        var builder = new ConfigurationBuilder()
                            .SetBasePath(hostEnvironment.ContentRootPath)
                            .AddJsonFile("appsettings.json", true, true)
                            .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                            .AddEnvironmentVariables();

        this.Configuration = builder.Build();

        this.Env = hostEnvironment;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        var apiconfiguration = BootstrapConfiguration.GetEnvConfiguration(Configuration);

        services.AddSingleton(apiconfiguration);

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

        services.AddDbContext<SampleDbContext>(opt => opt.UseSqlite(Configuration.GetConnectionString("SqlLiteConnectionStrings")));

        services.AddHttpContextAndUserAccessor()
                .AddFluentValidation()
                .AddServices()
                .AddRepositories()
                .AddDomainValidation()
                .AddHttpClients(apiconfiguration)
                .AddGrpcServices(apiconfiguration);
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SampleDbContext db)
    {
        //db.Database.Migrate();
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseRouting();
        //app.UseHttpsRedirection();
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

