
using VkNet.Model;
using VkNet;
using VkPostReader.TextParser;
using VkNet.Model.RequestParams;
using VkPostReader.VkPostsReader;
using Npgsql;
using VkPostReader;
using Microsoft.EntityFrameworkCore;

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
