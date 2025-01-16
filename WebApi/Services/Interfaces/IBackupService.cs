namespace WebApi.Services.Interfaces;

public interface IBackupService
{
    Task CreateBackupAsync(string filePath);
    Task RestoreFromBackupAsync(string filePath);
}