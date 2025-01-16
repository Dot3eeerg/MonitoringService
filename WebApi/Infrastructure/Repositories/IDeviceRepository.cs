using WebApi.Models;

namespace WebApi.Infrastructure.Repositories;

public interface IDeviceRepository
{
    Task<Device?> GetByIdAsync(Guid id);
    Task<IEnumerable<Device>> GetAllAsync();
    Task<Device> AddOrUpdateDeviceAsync(Device device, Session session);
    public Task<Device> GetSessionsByNameAsync(Guid id, string name);
}