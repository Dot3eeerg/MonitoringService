using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly WebApiDbContext _context;
    private readonly ILogger<DeviceRepository> _logger;

    public DeviceRepository(WebApiDbContext context, ILogger<DeviceRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Device?> GetByIdAsync(Guid id)
    {
        return await _context.Devices
            .Include(d => d.Sessions)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Device>> GetAllAsync()
    {
        return await _context.Devices
            .Include(d => d.Sessions)
            .ToListAsync();
    }

    public async Task<Device> AddOrUpdateDeviceAsync(Device device, Session session)
    {
        var existingDevice = await _context.Devices
            .Include(d => d.Sessions)
            .FirstOrDefaultAsync(d => d.Id == device.Id);

        if (existingDevice == null)
        {
            _context.Devices.Add(device);
            _context.Sessions.Add(session);
            _logger.LogInformation("Device {DeviceId} is added", device.Id);
        }
        else
        {
            _context.Sessions.Add(session);
            _context.Entry(existingDevice).CurrentValues.SetValues(device);
        
            _logger.LogInformation("Device {DeviceId} is updated", device.Id);
        }
    
        await _context.SaveChangesAsync();
        return device;
    }

    public async Task<IEnumerable<Session>> GetSessionsByNameAsync(Guid id, string name)
    {
        var device = await GetByIdAsync(id);
        
        if (device == null)
        {
            throw new KeyNotFoundException("Device not found");
            // throw new DeviceNotFoundException
        }

        return device.Sessions
            .Where(s => s.Name == name)
            .ToList();
    }
}