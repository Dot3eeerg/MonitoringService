using System.Text.Json.Serialization;

namespace WebApi.Models.DTO;

public record SessionForCreationDto
{
    [JsonPropertyName("_id")]
    [JsonConverter(typeof(GuidParsingConverter))]
    public Guid Id { get; init; }

    public string Name { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Version { get; init; }
}
