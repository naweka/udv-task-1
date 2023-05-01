
using VkNet.Model;
using VkNet;
using VkPostReader.TextParser;
using VkNet.Model.RequestParams;
using VkPostReader.VkPostsReader;
using Npgsql;
using VkPostReader;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITextConverter, TextConverter>();
builder.Services.AddSingleton<IVkPostsReader, VkPostsReader>();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(config["ConnectionString"]);
});

Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(dispose: true);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
