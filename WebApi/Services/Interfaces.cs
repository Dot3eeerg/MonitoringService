using WebApi.Models;
using WebApi.Models.DTO;

namespace WebApi.Services;

public interface IDeviceService
{
    Task<DeviceDto> AddSessionAsync(Guid id, string name, DateTime startTime, DateTime endTime, string version);
    Task<DeviceDto?> GetDeviceByIdAsync(Guid id);
    Task<IEnumerable<DeviceDto>> GetAllDevicesAsync();
}

public interface IServiceManager
{
    IDeviceService DeviceService { get; }
}