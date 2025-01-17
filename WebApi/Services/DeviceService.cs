using MapsterMapper;
using WebApi.Infrastructure.Repositories;
using WebApi.Models;
using WebApi.Models.DTO;
using WebApi.Services.Interfaces;

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
            _logger.LogWarning("Attempted to create session with empty name for device {DeviceId}",
                sessionForCreation.Id);
            throw new ArgumentException("Name cannot be null or empty");
        }

        if (sessionForCreation.StartTime >= sessionForCreation.EndTime)
        {
            _logger.LogWarning(
                "Invalid session time range for device {DeviceId}: Start {StartTime}, End {EndTime}", 
                sessionForCreation.Id, 
                sessionForCreation.StartTime, 
                sessionForCreation.EndTime);
            throw new ArgumentException("Start time cannot be greater than end time");
        }
        
        _logger.LogDebug("Retrieving device {DeviceId}", sessionForCreation.Id);
        var device = await _deviceRepository.GetByIdAsync(sessionForCreation.Id);
    
        if (device == null)
        {
            _logger.LogInformation("Creating new device with ID {DeviceId}", sessionForCreation.Id);
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
    
        _logger.LogDebug("Adding new session {SessionId} to device {DeviceId}", 
            session.Id, device.Id);
        device.Sessions.Add(session);
    
        var updatedDevice = await _deviceRepository.AddOrUpdateDeviceAsync(device, session);
        
        _logger.LogInformation(
            "Successfully added session {SessionId} for device {DeviceId} ({Name}: {StartTime} - {EndTime})", 
            session.Id,
            device.Id,
            session.Name,
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
        }
        
        _logger.LogInformation("Getting device usage information from {DeviceId}", device.Id);
        
        var deviceDto = _mapper.Map<DeviceDto>(device);
        return deviceDto;
    }

    public async Task<IEnumerable<DeviceDto>> GetAllDevicesAsync()
    {
        _logger.LogDebug("Retrieving all devices");
        var devices = await _deviceRepository.GetAllAsync();
        
        _logger.LogInformation("Retrieved {DeviceCount} devices", devices.Count());
        
        var devicesDto = _mapper.Map<IEnumerable<Device>, IEnumerable<DeviceDto>>(devices);
        return devicesDto;
    }

    public async Task<DeviceDto> GetSessionsByNameAsync(Guid id, string name)
    {
        _logger.LogDebug("Retrieving sessions for device {DeviceId} and user {Name}", id, name);
        var deviceUserSessions = await _deviceRepository.GetSessionsByNameAsync(id, name);

        if (deviceUserSessions == null)
        {
            _logger.LogWarning("No sessions found for device {DeviceId} and user {Name}", id, name);
            throw new KeyNotFoundException("Session not found");
        }
        
        _logger.LogInformation(
            "Retrieved {SessionCount} sessions for device {DeviceId} and user {Name}", 
            deviceUserSessions.Sessions.Count,
            id,
            name);
        
        var deviceUserSessionsDto = _mapper.Map<Device, DeviceDto>(deviceUserSessions);
        return deviceUserSessionsDto;
    }

    public async Task DeleteSessionAsync(Guid deviceId, Guid sessionId)
    {
        _logger.LogDebug("Retrieving sessions for device {DeviceId}", deviceId);
        var device = await _deviceRepository.GetByIdAsync(deviceId);

        if (device == null)
        {
            _logger.LogWarning("No device with {DeviceId} was found", deviceId);
            throw new KeyNotFoundException("Device not found");
        }

        var session = await _deviceRepository.GetSessionByIdAsync(sessionId);

        if (session == null)
        {
            _logger.LogWarning("No session with {SessionId} was found", sessionId);
            throw new KeyNotFoundException("Session not found");
        }

        await _deviceRepository.DeleteSessionAsync(session);
        
        _logger.LogInformation("Session {SessionId} deleted from device {DeviceId}", sessionId, deviceId);
    }
}