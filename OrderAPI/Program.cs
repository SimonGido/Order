using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;

using OrderAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IOrderService, OrderServiceDb>();
builder.Services.AddDbContext<OrderServiceDb>();


builder.Services.AddHangfire(config => config.UseMemoryStorage());
builder.Services.AddHangfireServer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// For testing purposes make sure database is always recreated
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderServiceDb>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
    OrderAPI.TestSetupShop.CreateTestProducts(dbContext);
}

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
