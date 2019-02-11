using System.Collections.Specialized;

namespace Common.Exception
{
    public interface IResultException
    {
        int StatusCode { get; }

        string ErrorMessage { get; }

        object ErrorData { get; }

        //NameValueCollection MethodParams { get; }
    }
}
