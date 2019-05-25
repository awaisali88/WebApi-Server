using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Dapper.Repositories.Extensions
{
    internal static class TypeExtensions
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _reflectionPropertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static PropertyInfo[] FindClassProperties(this Type objectType)
        {
            if (_reflectionPropertyCache.ContainsKey(objectType))
                return _reflectionPropertyCache[objectType];

            var propertyInfos = objectType.GetProperties()
                .OrderBy(p => p.GetCustomAttributes<ColumnAttribute>()
                    .Select(a => a.Order)
                    .DefaultIfEmpty(int.MaxValue)
                    .FirstOrDefault()).ToArray();

            _reflectionPropertyCache.TryAdd(objectType, propertyInfos);

            return propertyInfos;
        }

        public static dynamic ToDynamic(this IDictionary<string, object> dictionary)
        {
            var expandoObj = new ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<string, object>>)expandoObj;

            foreach (var keyValuePair in dictionary)
            {
                expandoObjCollection.Add(keyValuePair);
            }
            dynamic eoDynamic = expandoObj;
            return eoDynamic;
        }

        public static T ToObject<T>(this IDictionary<string, object> source)
            where T : class
        {
            var someObject = Activator.CreateInstance<T>();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType
                    .GetProperty(item.Key)
                    .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static T ToObject<T>(this IDictionary<string, object> source, T destination)
            where T : class
        {
            var destObjectType = destination.GetType();

            foreach (var item in source)
            {
                destObjectType
                    .GetProperty(item.Key)
                    .SetValue(destination, item.Value, null);
            }

            return destination;
        }

        public static IDictionary<string, object> ToDictionary<T>(this T data) where T : class
        {
            var allProperties = data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var properties = new List<PropertyInfo>();
            foreach (var propertyInfo in allProperties)
            {
                if (!propertyInfo.GetCustomAttributes().Any() || !propertyInfo.GetCustomAttributes().Any(x => x.GetType() == typeof(NotMappedAttribute)))
                {
                    if (propertyInfo.GetValue(data) is DateTime? && (!(propertyInfo.GetValue(data) is DateTime?) || propertyInfo.GetValue(data) as DateTime? == DateTime.MinValue))
                    {
                        propertyInfo.SetValue(data, new DateTime(1753, 1, 1));
                    }
                    properties.Add(propertyInfo);
                }
            }
            return properties.ToDictionary(prop => prop.Name, prop => prop.GetValue(data));
        }

        public static bool IsGenericType(this Type type)
        {
#if NESTANDARD13

            return type.GetTypeInfo().IsGenericType;
#else
            return type.IsGenericType;
#endif
        }

        public static bool IsEnum(this Type type)
        {
#if NESTANDARD13
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        public static bool IsValueType(this Type type)
        {
#if NESTANDARD13
            return type.GetTypeInfo().IsValueType;
#else
            return type.IsValueType;
#endif
        }

        public static bool IsBool(this Type type)
        {
            return type == typeof(bool);
        }
    }
}
