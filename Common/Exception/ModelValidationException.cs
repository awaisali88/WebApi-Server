using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Common.Exception
{
    [Serializable]
    public class ModelValidationException : System.Exception
    {
        public ModelValidationException()
        {
            StatusCode = 400;
            ErrorData = null;
        }

        public ModelValidationException(string message) : base(message)
        {
            StatusCode = 400;
            ErrorMessage = message;
            ErrorData = null;
        }

        public ModelValidationException(string message, List<ErrorsModelException> errorData) : base(message)
        {
            StatusCode = 400;
            ErrorMessage = message;
            ErrorData = errorData;
        }

        protected ModelValidationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public int StatusCode { get; }
        public string ErrorMessage { get; }
        public List<ErrorsModelException> ErrorData { get; }
    }

    public class ErrorsModelException
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }
}
