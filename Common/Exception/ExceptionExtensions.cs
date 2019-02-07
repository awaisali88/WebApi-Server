using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Exception
{
    public static class ExceptionExtensions
    {
        public static void ThrowIfNull<T>(this T @object, string paramName)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(paramName, $"Parameter {paramName} cannot be null.");
            }
        }
    }
}
