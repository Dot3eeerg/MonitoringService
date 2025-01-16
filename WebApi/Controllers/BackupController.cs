using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BackupController : ControllerBase
{
    private IBackupService _backupService;
    private IConfiguration _configuration;
    private ILogger<BackupController> _logger;

    public BackupController(IBackupService backupService, IConfiguration configuration, ILogger<BackupController> logger)
    {
        _backupService = backupService;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBackup()
    {
        try
        {
            var backupPath = Path.Combine(_configuration["BackupSettings:Path"] ?? "Backups",
                $"backup_{DateTime.Now:yyyyMMddHHmmss}.json");
            
            Directory.CreateDirectory(Path.GetDirectoryName(backupPath)!);
            
            _logger.LogInformation("Starting backup creation to {BackupPath}", backupPath);
            await _backupService.CreateBackupAsync(backupPath);
            _logger.LogInformation("Backup successfully created at {BackupPath}", backupPath);
            
            return Ok(new { message = "Backup created successfully", path = backupPath });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create backup");
            return StatusCode(500, "An error occurred while creating backup");
        }

        
    }

    [HttpPost("restore")]
    public async Task<IActionResult> RestoreBackup([FromQuery] string fileName)
    {
        try
        {
            var backupPath = Path.Combine(_configuration["BackupSettings:Path"] ?? "Backups", fileName);
            
            _logger.LogInformation("Starting restoration from backup {BackupPath}", backupPath);
            await _backupService.RestoreFromBackupAsync(backupPath);
            _logger.LogInformation("Successfully restored from backup {BackupPath}", backupPath);
            
            return Ok(new { message = "Backup restored successfully", path = backupPath });
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning(ex, "Backup file not found: {FileName}", fileName);
            return NotFound($"Backup file not found: {fileName}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restore from backup {FileName}", fileName);
            return StatusCode(500, "An error occurred while restoring backup");
        }

    }
}