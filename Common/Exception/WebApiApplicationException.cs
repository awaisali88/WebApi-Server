using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.Serialization;

namespace Common.Exception
{
    [Serializable]
    public class WebApiApplicationException : System.Exception, IResultException
    {
        public WebApiApplicationException()
        {
            StatusCode = 500;
            ErrorData = null;
        }

        public WebApiApplicationException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
            ErrorMessage = message;
            ErrorData = null;
        }

        public WebApiApplicationException(int statusCode, string message, object errorData) : base(message)
        {
            StatusCode = statusCode;
            ErrorMessage = message;
            ErrorData = errorData;
        }

        public WebApiApplicationException(string message) : base(message)
        {
            StatusCode = 500;
            ErrorMessage = message;
            ErrorData = null;
        }

        public WebApiApplicationException(string message, System.Exception inner) : base(message, inner)
        {
            StatusCode = 500;
            ErrorMessage = message;
            ErrorData = null;
        }

        protected WebApiApplicationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public int StatusCode { get; }
        public string ErrorMessage { get; }
        public object ErrorData { get; }

        //public NameValueCollection MethodParams => throw new NotImplementedException();
    }
}
