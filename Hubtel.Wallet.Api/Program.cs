using Hubtel.Wallet.Api.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("./Logs/WalletApi.txt", rollingInterval: RollingInterval.Day).CreateLogger();

// Add services to the container.
builder.Services.AddApiServices(builder.Configuration);
builder.Host.UseSerilog();

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
