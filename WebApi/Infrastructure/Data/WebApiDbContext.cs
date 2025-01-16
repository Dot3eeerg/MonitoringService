using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Infrastructure.Data;

public class WebApiDbContext : DbContext
{
    public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options) { }
    
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Session> Sessions => Set<Session>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var device1 = new Device
        {
            Id = Guid.Parse("f695ea23-8662-4a57-975a-f5afd26655db")
        };

        var session1 = new Session
        {
            Id = Guid.NewGuid(),
            DeviceId = device1.Id,
            Name = "John Doe",
            StartTime = DateTime.UtcNow.AddHours(-1),
            EndTime = DateTime.UtcNow,
            Version = "1.0.0.56"
        };
        
        var session2 = new Session
        {
            Id = Guid.NewGuid(),
            DeviceId = device1.Id,
            Name = "John Doe",
            StartTime = DateTime.UtcNow.AddHours(-3),
            EndTime = DateTime.UtcNow.AddHours(-2),
            Version = "1.0.0.57"
        };
        
        var session3 = new Session
        {
            Id = Guid.NewGuid(),
            DeviceId = device1.Id,
            Name = "Kaladin Stormblessed",
            StartTime = DateTime.UtcNow.AddHours(-4),
            EndTime = DateTime.UtcNow.AddHours(-3),
            Version = "1.0.0.57"
        };
        
        var device2 = new Device
        {
            Id = Guid.Parse("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f")
        };

        var session4 = new Session
        {
            Id = Guid.NewGuid(),
            DeviceId = device2.Id,
            Name = "Jane Smith",
            StartTime = DateTime.UtcNow.AddHours(-2),
            EndTime = DateTime.UtcNow.AddHours(-1),
            Version = "1.0.0.56"
        };


        modelBuilder.Entity<Device>().HasData(device1, device2);
        modelBuilder.Entity<Session>().HasData(session1, session2, session3, session4);
    }
}