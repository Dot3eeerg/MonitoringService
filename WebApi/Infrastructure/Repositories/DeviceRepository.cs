using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;
using WebApi.Models;

namespace WebApi.Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly WebApiDbContext _context;

    public DeviceRepository(WebApiDbContext context)
    {
        _context = context;
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
        }
        else
        {
            _context.Sessions.Add(session);
            _context.Entry(existingDevice).CurrentValues.SetValues(device);
        }
    
        await _context.SaveChangesAsync();
        return device;
    }

    public async Task<Device> GetSessionsByNameAsync(Guid id, string name)
    {
        var device = await _context.Devices.Include(d => d.Sessions.Where(n => n.Name == name))
            .FirstOrDefaultAsync(d => d.Id == id);
        
        if (device == null)
        {
            throw new KeyNotFoundException("Device not found");
        }

        return device;
    }
}