using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI_Server.Middleware
{
    /// <inheritdoc />
    public class AngularAntiforgeryCookieResultFilter : ResultFilterAttribute
    {
        private readonly IAntiforgery _antiforgery;
        internal AngularAntiforgeryCookieResultFilter(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        /// <inheritdoc />
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //string path = context.Request.Path.Value;

            //List<bool> checkOpenUrls = new List<bool>();
            //foreach (var url in OpenUrls.Urls)
            //{
            //    checkOpenUrls.Add(string.Equals(path, url, StringComparison.OrdinalIgnoreCase));
            //}
            ////if (
            ////    string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) ||
            ////    string.Equals(path, "/errorlog", StringComparison.OrdinalIgnoreCase) ||
            ////    string.Equals(path, "/index.html", StringComparison.OrdinalIgnoreCase))
            //if (checkOpenUrls.Any())
            //{
            //    // The request token can be sent as a JavaScript-readable cookie, 
            //    // and Angular uses it by default.
            //    var tokens = antiforgery.GetAndStoreTokens(context);
            //    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
            //        new CookieOptions() { HttpOnly = false });
            //}

            if (context.Result is ViewResult)
            {
                var tokens = _antiforgery.GetAndStoreTokens(context.HttpContext);
                context.HttpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false, Secure = true });
            }
        }
    }
}
