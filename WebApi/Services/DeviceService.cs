using MapsterMapper;
using WebApi.Models;
using WebApi.Models.DTO;
using WebApi.Repositories;

namespace WebApi.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly ILogger<DeviceService> _logger;
    private readonly IMapper _mapper;

    public DeviceService(IDeviceRepository deviceRepository, ILogger<DeviceService> logger, IMapper mapper)
    {
        _deviceRepository = deviceRepository;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<DeviceDto> AddSessionAsync(Guid id, string name, DateTime startTime, DateTime endTime, string version)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be null or empty");
        }

        if (startTime >= endTime)
        {
            throw new ArgumentException("Start time cannot be greater than end time");
        }

        var device = await _deviceRepository.GetByIdAsync(id);
        if (device == null)
        {
            device = new Device
            {
                Id = id
            };
        }

        var session = new Session
        {
            Id = Guid.NewGuid(),
            DeviceId = device.Id,
            Name = name,
            StartTime = startTime,
            EndTime = endTime,
            Version = version
        };
        
        device.Sessions.Add(session);
        
        await _deviceRepository.AddOrUpdateDeviceAsync(device);

        _logger.LogInformation("Added new session for user {Name} from {StartTime} to {EndTime}", name, startTime,
            endTime);
        
        var deviceDto = _mapper.Map<DeviceDto>(device);
        return deviceDto;
    }

    public async Task<DeviceDto?> GetDeviceByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DeviceDto>> GetAllDevicesAsync()
    {
        var devices = await _deviceRepository.GetAllAsync();
        
        _logger.LogInformation("Getting all devices");
        
        var devicesDto = _mapper.Map<IEnumerable<Device>, IEnumerable<DeviceDto>>(devices);
        return devicesDto;
    }
}