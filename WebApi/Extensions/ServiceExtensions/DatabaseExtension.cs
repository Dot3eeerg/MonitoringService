using WebApi.Infrastructure.Data;

namespace WebApi.Extensions.ServiceExtensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WebApiDbContext>();
        await context.Database.EnsureCreatedAsync();
    }
}
