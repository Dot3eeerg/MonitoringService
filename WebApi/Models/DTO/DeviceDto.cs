namespace WebApi.Models.DTO;

[Serializable]
public record DeviceDto(Guid Id, IEnumerable<SessionDto> Sessions);