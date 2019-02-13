using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common
{
    public static class ExtensionMethods
    {
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        // Convert a byte array to an Object
        public static object ToObject(this byte[] arrBytes)
        {
            if (arrBytes == null)
                return null;
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        /// <summary>
        /// Match string to string with Like Method eg. "Abc DE" equals "ab" OR "cd" OR "abcde"
        /// </summary>
        /// <param name="str">string value to match from</param>
        /// <param name="equals">string value to match</param>
        /// <returns></returns>
        public static bool Like(this string str, string equals)
        {
            for (; ; )
            {
                if (str.Contains(" "))
                    str = str.Replace(" ", "");
                else
                    break;
            }
            return equals.ToLower().Contains(str.ToLower());
        }

        /// <summary>
        /// Checck IEnumerable for Null Or Empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            /* If this is a list, use the Count property for efficiency. 
             * The Count property is O(1) while IEnumerable.Count() is O(N). */
            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return !enumerable.Any();
        }

        /// <summary>
        /// Get String value from DescriptionAttribute from any Property or Enum Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescriptionString<T>(this T value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        /// Convert any object to Dynamic object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }

        public static T? ToNullable<T>(this string s) where T : struct
        {
            T? result = new T?();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    var convertFrom = conv.ConvertFrom(s);
                    if (convertFrom != null) result = (T)convertFrom;
                }
            }
            catch
            {
                // ignored
            }
            return result;
        }

        #region Enum
        /// <summary>
        /// Get Enum Value form String value in StringAttribute
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <param name="stringValue">StringAttribute Value</param>
        /// <returns></returns>
        public static T GetEnumValue<T>(this string stringValue)
        {
            if (!string.IsNullOrEmpty(stringValue))
            {
                if (stringValue.ToLower().Contains("sample"))
                {
                    stringValue = "M";
                }
                var type = typeof(T);
                if (!type.IsEnum) throw new InvalidOperationException();
                foreach (var field in type.GetFields())
                {
                    if (Attribute.GetCustomAttribute(field,
                        typeof(StringValueAttribute)) is StringValueAttribute attribute)
                    {
                        if (attribute.Value == stringValue)
                            return (T)field.GetValue(null);
                    }
                    else
                    {
                        if (field.Name == stringValue)
                            return (T)field.GetValue(null);
                    }
                }
                throw new ArgumentException("Not found.", nameof(stringValue));
            }
            return default(T);
        }

        /// <summary>
        /// Get StringAttribute Value from Enum
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <param name="value">Enum value</param>
        /// <returns></returns>
        public static string GetEnumValue<T>(this T value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                StringValueAttribute[] attributes =
                    (StringValueAttribute[])fi.GetCustomAttributes(
                        typeof(StringValueAttribute),
                        false);

                if (attributes.Length > 0)
                    return attributes[0].Value;

                return value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get Enum Value from int value of Enum
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <param name="value">int value of Enum</param>
        /// <returns></returns>
        public static T EnumValue<T>(this int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        /// <summary>
        /// Get int value of Enum value
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <param name="value">Enum Value</param>
        /// <returns></returns>
        public static int EnumValue<T>(this T value) where T : struct, IConvertible
        {
            return (int)Enum.Parse(value.GetType(), value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Get Enum value form string of Enum
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <param name="name">Enum Value in string</param>
        /// <returns></returns>
        public static T StringToEnum<T>(this string name)
        {
            return (T)Enum.Parse(typeof(T), name);
        }
        #endregion

        #region KeyValue Pair
        public static KeyValuePair<string, T[]> PairedWith<T>(this string key, T[] value)
        {
            return new KeyValuePair<string, T[]>(key, value);
        }

        public static KeyValuePair<string, T> PairedWith<T>(this string key, T value)
        {
            return new KeyValuePair<string, T>(key, value);
        }

        public static KeyValuePair<string, object> PairedWith(this string key, object value)
        {
            return new KeyValuePair<string, object>(key, value);
        }
        #endregion

        #region DateTime
        public static DateTime SetTimeZone(this DateTime datetime, string timeZone = "")
        {
            if (!string.IsNullOrEmpty(timeZone))
            {
                var customZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                var newDate = TimeZoneInfo.ConvertTime(datetime, TimeZoneInfo.Local, customZone);
                return newDate;
            }
            return datetime.ToUniversalTime();
        }

        public static DateTime TrimMilliseconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0);
        }

        /// <summary>
        /// Gets the 12:00:00 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// Gets the 11:59:59 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteEnd(this DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }
        #endregion

        public static T GetFirstHeaderValueOrDefault<T>(this HttpRequestMessage message, string headerKey,
            Func<HttpRequestMessage, string> defaultValue,
            Func<string, T> valueTransform)
        {
            IEnumerable<string> headerValues;
            if (!message.Headers.TryGetValues(headerKey, out headerValues))
                return valueTransform(defaultValue(message));
            string firstHeaderValue = headerValues.FirstOrDefault() ?? defaultValue(message);
            return valueTransform(firstHeaderValue);
        }

        public static TValue GetAttributeValue<TAttribute, TValue>(
                this Type type,
                Func<TAttribute, TValue> valueSelector)
                where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }
    }
}
