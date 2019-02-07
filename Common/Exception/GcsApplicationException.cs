using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.Serialization;

namespace Common.Exception
{
    [Serializable]
    public class GcsApplicationException : System.Exception, IResultException
    {
        public GcsApplicationException()
        {
            StatusCode = 500;
            ErrorData = null;
        }

        public GcsApplicationException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
            ErrorMessage = message;
            ErrorData = null;
        }

        //public GcsApplicationException(int statusCode, string message, ParameterInfo[] parameters) : base(message)
        //{
        //    StatusCode = statusCode;
        //    ErrorMessage = message;
        //    ErrorData = null;
        //}

        public GcsApplicationException(int statusCode, string message, object errorData) : base(message)
        {
            StatusCode = statusCode;
            ErrorMessage = message;
            ErrorData = errorData;
        }

        //public GcsApplicationException(int statusCode, string message, ParameterInfo[] parameters, object errorData) : base(message)
        //{
        //    StatusCode = statusCode;
        //    ErrorMessage = message;
        //    ErrorData = errorData;
        //}

        public GcsApplicationException(string message) : base(message)
        {
            StatusCode = 500;
            ErrorMessage = message;
            ErrorData = null;
        }

        public GcsApplicationException(string message, System.Exception inner) : base(message, inner)
        {
            StatusCode = 500;
            ErrorMessage = message;
            ErrorData = null;
        }

        protected GcsApplicationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public int StatusCode { get; }
        public string ErrorMessage { get; }
        public object ErrorData { get; }

        public NameValueCollection MethodParams => throw new NotImplementedException();
    }
}
