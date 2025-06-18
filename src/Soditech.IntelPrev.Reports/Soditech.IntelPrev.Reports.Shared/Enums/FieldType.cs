using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Soditech.IntelPrev.Reports.Shared.Enums;

public enum FieldType
{
    Text,
    Number,
    Date,
    Time,
    DateTime,
    Boolean,
    Select,
    MultiSelect,
    List,
}

public class FieldTypeJsonConverter : JsonConverter<FieldType>
{
    public override FieldType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return Enum.TryParse<FieldType>(value, true, out var fieldType)
            ? fieldType
            : throw new JsonException($"Invalid value '{value}' for FieldType");
    }

    public override void Write(Utf8JsonWriter writer, FieldType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
