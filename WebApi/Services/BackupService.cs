using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;
using WebApi.Models;
using WebApi.Models.Backup;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class BackupService : IBackupService
{
    private readonly WebApiDbContext _context;
    private readonly ILogger<BackupService> _logger;

    public BackupService(WebApiDbContext context, ILogger<BackupService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task CreateBackupAsync(string filePath)
    {
        try
        {
            _logger.LogDebug("Retrieving sessions for backup");
            var backupData = new BackupData
            {
                Sessions = await _context.Sessions.ToListAsync(),
            };
            
            _logger.LogInformation("Creating backup with {SessionCount} sessions", backupData.Sessions.Count);

            var jsonString = JsonSerializer.Serialize(backupData, new JsonSerializerOptions
            {
                WriteIndented = true,
            });

            await File.WriteAllTextAsync(filePath, jsonString);
            _logger.LogInformation("Backup file created successfully at {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create backup: {FilePath}", filePath);
            throw;
        }
    }

    public async Task RestoreFromBackupAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Backup file not found at {FilePath}", filePath);
                throw new FileNotFoundException("File not found", filePath);
            }
            
            _logger.LogInformation("Reading backup file from {FilePath}", filePath);
            var jsonString = await File.ReadAllTextAsync(filePath);
            var backupData = JsonSerializer.Deserialize<BackupData>(jsonString);

            if (backupData == null)
            {
                _logger.LogError("Invalid backup data in file {FilePath}", filePath);
                throw new InvalidDataException("Backup data could not be deserialized");
            }

            _logger.LogInformation("Starting database restoration with {SessionCount} sessions",
                backupData.Sessions.Count);

            _logger.LogDebug("Clearing existing database records");
            _context.Sessions.RemoveRange(_context.Sessions);
            _context.Devices.RemoveRange(_context.Devices);

            var devices = backupData.Sessions.Select(s => s.DeviceId).Distinct()
                .Select(deviceId => new Device { Id = deviceId }).ToList();
            
            _logger.LogDebug("Adding {DeviceCount} devices from backup", devices.Count);
            await _context.Devices.AddRangeAsync(devices);
            
            _logger.LogDebug("Adding {SessionCount} sessions from backup", backupData.Sessions.Count);
            await _context.Sessions.AddRangeAsync(backupData.Sessions);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Database successfully restored from backup");
        }
        catch
        {
            _logger.LogError("Failed to restore from backup: {FilePath}", filePath);
            throw;
        }
    }
}