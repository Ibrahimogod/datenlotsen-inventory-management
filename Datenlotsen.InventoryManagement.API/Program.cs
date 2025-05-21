using System.Text.Json.Serialization;
using Datenlotsen.InventoryManagement;
using Datenlotsen.InventoryManagement.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<StockStatus>());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInventoryManagementServices(builder.Configuration.GetConnectionString("InventoryManagementDb"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();