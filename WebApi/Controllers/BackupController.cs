using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BackupController : ControllerBase
{
    private IBackupService _backupService;
    private IConfiguration _configuration;

    public BackupController(IBackupService backupService, IConfiguration configuration)
    {
        _backupService = backupService;
        _configuration = configuration;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBackup()
    {
        var backupPath = Path.Combine(_configuration["BackupSettings:Path"] ?? "Backups",
            $"backup_{DateTime.Now:yyyyMMddHHmmss}.json");
        
        Directory.CreateDirectory(Path.GetDirectoryName(backupPath)!);
        
        await _backupService.CreateBackupAsync(backupPath);
        return Ok(new { message = "Backup created successfully", path = backupPath });
    }

    [HttpPost("restore")]
    public async Task<IActionResult> RestoreBackup([FromQuery] string fileName)
    {
        var backupPath = Path.Combine(_configuration["BackupSettings:Path"] ?? "Backups", fileName);
        
        await _backupService.RestoreFromBackupAsync(backupPath);
        return Ok(new { message = "Backup restored successfully", path = backupPath });
    }
}