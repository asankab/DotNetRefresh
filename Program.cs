using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pros.Database;
using Pros.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWeatherClient, OpenWeatherClient>();
builder.Services.AddSingleton<Setting, Setting>();

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

builder.Services.AddHttpClient("weatherapi", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
