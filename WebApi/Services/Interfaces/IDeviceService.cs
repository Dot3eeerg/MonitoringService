using WebApi.Models.DTO;

namespace WebApi.Services.Interfaces;

public interface IDeviceService
{
    Task<DeviceDto> AddSessionAsync(SessionForCreationDto session);
    Task<DeviceDto?> GetDeviceByIdAsync(Guid id);
    Task<IEnumerable<DeviceDto>> GetAllDevicesAsync();
    Task<DeviceDto> GetSessionsByNameAsync(Guid id, string name);
}

public interface IServiceManager
{
    IDeviceService DeviceService { get; }
}