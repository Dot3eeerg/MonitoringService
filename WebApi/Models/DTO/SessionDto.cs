namespace WebApi.Models.DTO;

public record SessionDto(Guid SessionId, string Name, DateTime StartTime, DateTime EndTime, string Version);