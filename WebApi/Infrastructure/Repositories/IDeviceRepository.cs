using WebApi.Models;

namespace WebApi.Repositories;

public interface IDeviceRepository
{
    Task<Device?> GetByIdAsync(Guid id);
    // Task<Device?> GetByNameAsync(Guid id, string name);
    Task<IEnumerable<Device>> GetAllAsync();
    // Task DeleteOutdatedSessionsAsync(TimeSpan threshold);
    Task<Device> AddOrUpdateDeviceAsync(Device device);
    // Task DeleteAsync(Guid id);

}