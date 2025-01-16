using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;
using WebApi.Infrastructure.Repositories;
using WebApi.Repositories;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApi.Extensions.ServiceExtensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services, string[] allowedOrigins) =>
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

    public static void ConfigureRepository(this IServiceCollection services) =>
        services.AddScoped<IDeviceRepository, DeviceRepository>();
    
    public static void ConfigureService(this IServiceCollection services) =>
        services.AddScoped<IDeviceService, DeviceService>();

    public static void ConfigureDbContext(this IServiceCollection services) =>
        services.AddDbContext<WebApiDbContext>(options => options.UseInMemoryDatabase("WebApiDb"));

    public static void ConfigureMapster(this IServiceCollection services)
    {
        var config = new TypeAdapterConfig();
        new RegisterMapper().Register(config);
        
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }

    public static void ConfigureBackupService(this IServiceCollection services) =>
        services.AddScoped<IBackupService, BackupService>();
}