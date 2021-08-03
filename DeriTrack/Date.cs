using System;
using System.Collections.Generic;
using DeriTrack.Domain.Result;
using Newtonsoft.Json;

namespace DeriTrack
{
    [JsonConverter(typeof(DateJsonConverter))]
    public class Date : ValueObject
    {
        private readonly DateTime _dateTime;

        public static Result<Date> Create(string date)
        {
            if (DateTime.TryParse(date, out var dateTime)) return new Date(dateTime);

            return Errors.General.InvalidData("invalid date format");
        }

        public Date(string date)
        {
            _dateTime = DateTime.Parse(date);
        }

        // required for IModelBinder failure where date is invalid - see WebApiConfig.DateModelBinder
        public Date()
        {
        }

        public Date(DateTime date)
        {
            _dateTime = date.Date;
        }

        public static implicit operator DateTime?(Date date)
        {
            return date?._dateTime;
        }

        public static implicit operator DateTime(Date date)
        {
            return date._dateTime;
        }

        public static implicit operator Date(DateTime dateTime)
        {
            return new Date(dateTime);
        }

        public static implicit operator string(Date date)
        {
            return date._dateTime.ToString("yyyy-MM-dd");
        }

        public override string ToString()
        {
            return this;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _dateTime.Year;
            yield return _dateTime.Month;
            yield return _dateTime.Day;
        }
    }

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
