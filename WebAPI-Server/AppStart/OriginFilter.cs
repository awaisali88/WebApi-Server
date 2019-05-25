using Common;
using Common.Exception;
using Common.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace WebAPI_Server.AppStart
{
    internal class OriginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey(HttpRequestHeaders.Origin) ||
                (context.HttpContext.Request.Headers.ContainsKey(HttpRequestHeaders.Origin) &&
                 string.IsNullOrEmpty(context.HttpContext.Request.Headers[HttpRequestHeaders.Origin])))
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest,
                    ErrorMessages.Forbidden);
        }
    }
}
