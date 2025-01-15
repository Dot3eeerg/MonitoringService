using Microsoft.EntityFrameworkCore;
using WebApi.Extensions.ServiceExtensions;
using WebApi.Infrastructure.Data;
using WebApi.Repositories;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins")!.Split(",");

builder.Services.ConfigureCors(allowedOrigins);
builder.Services.ConfigureRepository();
builder.Services.ConfigureService();
builder.Services.ConfigureDbContext();
builder.Services.ConfigureMapster();

var app = builder.Build();

await app.InitializeDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();