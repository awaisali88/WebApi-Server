using Common;
using Common.Exception;
using Common.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace WebAPI_Server.AppStart
{
    internal class AccessTokenFilter : ActionFilterAttribute
    {
        internal bool UseAccessToken { get; set; }

        internal string Policy { get; set; }

        internal AccessTokenFilter(bool useAccessToken = true)
        {
            UseAccessToken = useAccessToken;

            Policy = useAccessToken ? "All" : "None";
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey(HttpRequestHeaders.ApiKey) ||
                (context.HttpContext.Request.Headers.ContainsKey(HttpRequestHeaders.ApiKey) &&
                 string.IsNullOrEmpty(context.HttpContext.Request.Headers[HttpRequestHeaders.ApiKey])))
                throw new WebApiApplicationException(StatusCodes.Status412PreconditionFailed,
                    ErrorMessages.InvalidApiKey);
        }
    }

    internal class AllowNoAccessToken : ActionFilterAttribute
    {
    }

}
