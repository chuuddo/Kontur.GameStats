using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Kontur.GameStats.Server.Infrastructure
{
    public class DoubleFormatConverter : JsonConverter
    {
        public override bool CanRead => false;
        public override bool CanConvert(Type objectType) => objectType == typeof(double);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(string.Format(CultureInfo.InvariantCulture, "{0:N6}", value));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }
    }
}