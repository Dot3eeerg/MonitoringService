using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DTO;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllDevices()
    {
        try
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            return Ok(devices);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occured while getting the devices");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDeviceInfo(Guid id)
    {
        try
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            return Ok(device);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Device with ID: {id} not found");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occured while getting the device info");
        }
    }
    
    [HttpGet("{id}/{name}")]
    public async Task<IActionResult> GetSessionsByName(Guid id, string name)
    {
        try
        {
            var device = await _deviceService.GetSessionsByNameAsync(id, name);
            return Ok(device);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Sessions with name: {name} not found");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occured while getting the sessions by name");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddSession([FromBody] SessionForCreationDto? session)
    {
        try
        {
            if (session == null)
            {
                return BadRequest("Session object is null");
            }

            var deviceDto = await _deviceService.AddSessionAsync(session);
            return CreatedAtAction(
                nameof(GetDeviceInfo),
                new { id = deviceDto.Id },
                deviceDto);
        }
        catch (KeyNotFoundException)
        {
            return BadRequest("Username cannot be null or empty / session start time cannot be greater than end time");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occured while adding new session");
        }
    }

    [HttpDelete("{deviceId}/{sessionId}")]
    public async Task<ActionResult> DeleteDeviceInfo(Guid deviceId, Guid sessionId)
    {
        try
        {
            await _deviceService.DeleteSessionAsync(deviceId, sessionId);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Device with ID: {deviceId} not found / Session with Id: {sessionId} not found");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occured while deleting the session");
        }
    }
}