using application.api.Configurations;
using shared.api.Handlers;


namespace application.api.startup;

public class Startup
{
    private IWebHostEnvironment Env { get; set; }
    public IConfiguration Configuration { get; }

    public Startup(IWebHostEnvironment hostEnvironment)
    {
        var builder = new ConfigurationBuilder()
                            .SetBasePath(hostEnvironment.ContentRootPath)
                            .AddJsonFile("appsettings.json", true, true)
                            .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                            .AddEnvironmentVariables();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        this.Configuration = builder.Build();

        this.Env = hostEnvironment;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        var apiconfiguration = BootstrapConfiguration.GetEnvConfiguration(Configuration);

        services.AddSingleton(apiconfiguration);

        services.AddControllerAndMvcSettings()
                .AddDatabase(apiconfiguration, Configuration)
                .AddHttpContextAndUserAccessor()
                .AddFluentValidation()
                .AddServices()
                .AddRepositories()
                .AddDomainValidation()
                .AddHttpClients(apiconfiguration)
                .AddGrpcServices(apiconfiguration);
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
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

