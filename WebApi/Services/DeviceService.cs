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

    public async Task<DeviceDto> AddSessionAsync(SessionForCreationDto sessionForCreation)
    {
        if (string.IsNullOrEmpty(sessionForCreation.Name))
        {
            throw new ArgumentException("Name cannot be null or empty");
        }

        if (sessionForCreation.StartTime >= sessionForCreation.EndTime)
        {
            throw new ArgumentException("Start time cannot be greater than end time");
        }

        var device = await _deviceRepository.GetByIdAsync(sessionForCreation.Id);
    
        if (device == null)
        {
            device = new Device
            {
                Id = sessionForCreation.Id,
                Sessions = new List<Session>()
            };
        }

        var session = new Session
        {
            Id = Guid.NewGuid(),
            DeviceId = device.Id,
            Name = sessionForCreation.Name,
            StartTime = sessionForCreation.StartTime,
            EndTime = sessionForCreation.EndTime,
            Version = sessionForCreation.Version, 
        };
    
        device.Sessions.Add(session);
    
        var updatedDevice = await _deviceRepository.AddOrUpdateDeviceAsync(device, session);
    
        _logger.LogInformation(
            "Added new session {SessionId} for device {DeviceId} from {StartTime} to {EndTime}", 
            session.Id,
            device.Id,
            session.StartTime,
            session.EndTime);
    
        var deviceDto = _mapper.Map<DeviceDto>(updatedDevice);
        return deviceDto;
    }

    public async Task<DeviceDto?> GetDeviceByIdAsync(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);

        if (device == null)
        {
            throw new KeyNotFoundException("Device not found");
            // throw new DeviceNotFoundException
        }
        
        _logger.LogInformation("Getting device usage information from {DeviceId}", device.Id);
        
        var deviceDto = _mapper.Map<DeviceDto>(device);
        return deviceDto;
    }

    public async Task<IEnumerable<DeviceDto>> GetAllDevicesAsync()
    {
        var devices = await _deviceRepository.GetAllAsync();
        
        _logger.LogInformation("Getting all devices");
        
        var devicesDto = _mapper.Map<IEnumerable<Device>, IEnumerable<DeviceDto>>(devices);
        return devicesDto;
    }

    public async Task<IEnumerable<SessionDto?>> GetSessionsByNameAsync(Guid id, string name)
    {
        var sessions = await _deviceRepository.GetSessionsByNameAsync(id, name);

        if (sessions == null)
        {
            throw new KeyNotFoundException("Session not found");
            // throw new SessionsNotFoundException
        }
        
        _logger.LogInformation("Getting sessions from {@Sessions}", sessions);
        
        var sessionsDto = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionDto>>(sessions);
        return sessionsDto;
    }
}