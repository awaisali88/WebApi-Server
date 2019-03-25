using System;

// ReSharper disable once CheckNamespace
namespace WebAPI_Server
{
#pragma warning disable 1591
    public static class ExtMethods
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }
    }
#pragma warning restore 1591
}
