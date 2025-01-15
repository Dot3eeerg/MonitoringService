using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace WebApi;

public class GuidParsingConverter : JsonConverter<Guid>
{
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();

        if (Guid.TryParse(stringValue, out var guid))
        {
            return guid;
        }

        throw new JsonException($"Invalid GUID format: {stringValue}");
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
