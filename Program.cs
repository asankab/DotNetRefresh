using Microsoft.EntityFrameworkCore;
using Pros.Database;
using Pros.Options;
using Pros.Service;
using Serilog;
using MediatR;
using Pros.Application.Behaviours;
using System.Reflection;
using Microsoft.Extensions.Logging.AzureAppServices;
using HealthChecks.UI.Client;
using Pros.Exceptions;
using Microsoft.AspNetCore.ResponseCompression;

// https://www.milanjovanovic.tech/blog
var builder = WebApplication.CreateBuilder(args);

//object value = builder.Logging.AddAzureWebAppDiagnostics();
//builder.Services.Configure<AzureFileLoggerOptions>(options =>
//{
//    options.FileName = "azure-diagnostics-";
//    options.FileSizeLimit = 50 * 1024;
//    options.RetainedFileCountLimit = 5;
//});
//builder.Services.Configure<AzureBlobLoggerOptions>(options =>
//{
//    options.BlobName = "log.txt";
//});

builder.Services.AddHealthChecks()
    .AddCheck<CustomHealthCheck>("Custom");

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Services.AddTransient<GlobalExceptionHandler>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext();
});

builder.Services.AddScoped(
    typeof(IPipelineBehavior<,>),
    typeof(LoggingBehavior<,>));

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

builder.Services.AddResponseCompression(o =>
{
    o.EnableForHttps = true;
});

builder.Services.Configure<BrotliCompressionProviderOptions>(o =>
{
    o.Level = System.IO.Compression.CompressionLevel.Fastest;
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWeatherClient, OpenWeatherClient>();
builder.Services.AddSingleton<Setting, Setting>();

builder.Services.AddHealthChecks();

//builder.Services.AddDbContext<SchoolDatabaseContext>(
//    (serviceProvider, DbContextOptionsBuilder) =>
//    {
//        var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;
//        Setting.ConnectionString = databaseOptions.ConnectionString;
//        //var connectionString = builder.Configuration.GetConnectionString("Database");

//        DbContextOptionsBuilder.UseSqlServer(databaseOptions.ConnectionString, sqlServerAction =>
//        {
//            sqlServerAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
//            sqlServerAction.CommandTimeout(databaseOptions.CommandTimeout);
//        });

//        DbContextOptionsBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
//        DbContextOptionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSenstiveDataLogging);

//    });
builder.Services.AddDbContext<SchoolDatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDatabase"));
});

builder.Services.AddHttpClient("weatherapi", (serviceProvider, httpClient) =>
{
    //var githubSettings = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

    //httpClient.DefaultRequestHeaders.Add("Authorization", githubSettings.ConnectionString);
    //httpClient.DefaultRequestHeaders.Add("User-Agent", httpClient.BaseAddress.ToString());
    httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");

});

builder.Services.AddHttpClient<GitHubService>((serviceProvider, httpClient) =>
{
    //var githubSettings = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

    //httpClient.DefaultRequestHeaders.Add("Authorization", githubSettings.ConnectionString);
    //httpClient.DefaultRequestHeaders.Add("User-Agent", httpClient.BaseAddress.ToString());
    httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");

})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new SocketsHttpHandler
    {
        PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
    };
})
.SetHandlerLifetime(Timeout.InfiniteTimeSpan);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseSerilogRequestLogging();

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
