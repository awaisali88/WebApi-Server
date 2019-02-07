using Common;
using Common.Exception;
using Common.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI_Server.AppStart
{
    internal class RequireCallbackUrl : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey(HttpRequestHeaders.RequestUrl) ||
                (context.HttpContext.Request.Headers.ContainsKey(HttpRequestHeaders.RequestUrl) &&
                 string.IsNullOrEmpty(context.HttpContext.Request.Headers[HttpRequestHeaders.RequestUrl])))
                throw new WebApiApplicationException(StatusCodes.Status412PreconditionFailed,
                    ErrorMessages.InvalidRequestUrl);
        }
    }
}
