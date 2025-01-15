namespace WebApi.Models.DTO;

public record SessionDto(string Name, DateTime StartTime, DateTime EndTime, string Version);