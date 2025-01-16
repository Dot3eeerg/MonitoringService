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
            var backupData = new BackupData
            {
                Sessions = await _context.Sessions.ToListAsync(),
            };

            var jsonString = JsonSerializer.Serialize(backupData, new JsonSerializerOptions
            {
                WriteIndented = true,
            });

            await File.WriteAllTextAsync(filePath, jsonString);
            _logger.LogInformation("Created backup: {FilePath}", filePath);
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
                throw new FileNotFoundException("File not found", filePath);
            }

            var jsonString = await File.ReadAllTextAsync(filePath);
            var backupData = JsonSerializer.Deserialize<BackupData>(jsonString);

            if (backupData == null)
            {
                throw new InvalidDataException("Backup data could not be deserialized");
            }

            _context.Sessions.RemoveRange(_context.Sessions);
            _context.Devices.RemoveRange(_context.Devices);

            var devices = backupData.Sessions.Select(s => s.DeviceId).Distinct()
                .Select(deviceId => new Device { Id = deviceId }).ToList();

            await _context.Devices.AddRangeAsync(devices);
            await _context.Sessions.AddRangeAsync(backupData.Sessions);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Restored from backup: {FilePath}", filePath);
        }
        catch
        {
            _logger.LogError("Failed to restore from backup: {FilePath}", filePath);
            throw;
        }
    }
}