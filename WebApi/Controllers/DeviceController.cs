using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

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
        var devices = await _deviceService.GetAllDevicesAsync();
        return Ok(devices);
    }
    
    // [HttpGet("{id}")]
    // public ActionResult GetDeviceInfo(Guid id)
    // {
    //     
    // }
    //
    // [HttpPost]
    // public ActionResult CreateDeviceInfo([FromBody] DeviceDto deviceDto)
    // {
    //     
    // }
    //
    // [HttpDelete("{id}")]
    // public ActionResult DeleteDeviceInfo(Guid id)
    // {
    //     
    // }
}