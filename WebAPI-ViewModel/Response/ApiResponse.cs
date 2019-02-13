using Common;
using Microsoft.AspNetCore.Http;

namespace WebAPI_ViewModel.Response
{
    public class ApiResponse
    {
        private readonly object _error;
        private readonly HttpContext _httpContext;

        public ApiResponse(HttpContext httpContext, bool status, string msg, object data)
        {
            _httpContext = httpContext;
            Status = status;
            Msg = msg;
            Data = data;
        }
        public ApiResponse(bool status, string msg, object data, object error)
        {
            _httpContext = null;
            Status = status;
            Msg = msg;
            Data = data;
            _error = error;
        }
        public ApiResponse(bool status, string msg)
        {
            _httpContext = null;
            Status = status;
            Msg = msg;
            Data = null;
            _error = null;
        }

        public bool Status { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }
        //public object Error => _httpContext == null ? _error : _httpContext.Session.Get(AppConstants.SessionErrorKey).ToObject();
        public object Error
        {
            get
            {
                if (_httpContext == null)
                    return _error;
                object errorValue = _httpContext.Session.Get(AppConstants.SessionErrorKey).ToObject();
                _httpContext.Session.Remove(AppConstants.SessionErrorKey);
                return errorValue;
            }
        }
    }
    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(HttpContext httpContext, bool status, string msg, T data) : base(httpContext, status, msg, data)
        {
        }
        public ApiResponse(bool status, string msg, T data, object error) : base(status, msg, data, error)
        {
        }
        public new T Data { get => (T)base.Data; set => base.Data = value; }
    }
}
