using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.JsonConverter
{
    public class StringValueEnumConverter<T> : Newtonsoft.Json.JsonConverter where T : struct, IComparable, IConvertible, IFormattable 
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object val = reader.Value;

            if (val != null)
            {
                var enumString = (string)reader.Value;

                return enumString.GetEnumValue<T>();
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Enum sourceEnum)
            {
                string enumText = sourceEnum.GetEnumValue();
                writer.WriteValue(enumText);
            }
        }
    }
}
