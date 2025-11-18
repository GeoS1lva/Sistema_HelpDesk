using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sistema_HelpDesk.Desk.Infra.JSONSerializacao
{
    public class JsonDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DateTimeOffset.Parse(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss zzz"));
    }
}
