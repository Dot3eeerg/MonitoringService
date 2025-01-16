﻿using Microsoft.AspNetCore.Mvc;
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
        var devices = await _deviceService.GetAllDevicesAsync();
        return Ok(devices);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDeviceInfo(Guid id)
    {
        var device = await _deviceService.GetDeviceByIdAsync(id);
        return Ok(device);
    }
    
    [HttpGet("{id}/{name}")]
    public async Task<IActionResult> GetSessionsByName(Guid id, string name)
    {
        var device = await _deviceService.GetSessionsByNameAsync(id, name);
        return Ok(device);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddSession([FromBody] SessionForCreationDto? session)
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
    
    // [HttpDelete("{id}")]
    // public ActionResult DeleteDeviceInfo(Guid id)
    // {
    //     
    // }
}