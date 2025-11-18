using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sistema_HelpDesk.Desk.Infra.JSONSerializacao
{
    public class JsonTimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => TimeSpan.Parse(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(@"hh\:mm\:ss"));
    }
}
