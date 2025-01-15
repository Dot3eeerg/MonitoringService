using WebApi.Models;
using WebApi.Models.DTO;

namespace WebApi.Services;

public interface IDeviceService
{
    Task<DeviceDto> AddSessionAsync(SessionForCreationDto session);
    Task<DeviceDto?> GetDeviceByIdAsync(Guid id);
    Task<IEnumerable<DeviceDto>> GetAllDevicesAsync();
    Task<IEnumerable<SessionDto?>> GetSessionsByNameAsync(Guid id, string name);
}

public interface IServiceManager
{
    IDeviceService DeviceService { get; }
}