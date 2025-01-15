namespace WebApi.Models.DTO;

public record CreateSessionDto(string Name, DateTime StartTime, DateTime EndTime, string Version);
