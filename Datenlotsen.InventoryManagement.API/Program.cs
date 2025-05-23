using System.Text.Json.Serialization;
using Datenlotsen.InventoryManagement;
using Datenlotsen.InventoryManagement.Enums;
using Datenlotsen.InventoryManagement.API.Constants;
using Datenlotsen.InventoryManagement.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<StockStatus>());
});
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInventoryManagementServices(builder.Configuration.GetConnectionString(ConnectionStringNames.InventoryManagementDb)!);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();