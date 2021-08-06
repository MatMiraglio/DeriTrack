using System;
using Domain;
using Newtonsoft.Json;

namespace DeriTrack
{
    public class DateJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var date = (string)reader.Value;

            if (string.IsNullOrEmpty(date)) return null;

            var create = Date.Create(date);

            if (create.IsSuccess)
            {
                return create.Value;
            }

            throw new JsonException($"Unable to convert \"{date}\" to Date");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Date);
        }
    }
}