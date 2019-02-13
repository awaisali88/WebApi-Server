using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Exception;
using Common.Messages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using WebAPI_ViewModel.Response;

namespace WebAPI_Server.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        /// <exception cref="WebApiApplicationException"></exception>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                _logger.LogInformation("{@Path}", httpContext.Request.Path);
                if (OpenUrls.Urls.Any(x => httpContext.Request.Path.StartsWithSegments(x)) && !OpenUrls.WebUrls.Any(x => httpContext.Request.Path.StartsWithSegments(x)))
                {
                    AuthenticateResult result = await httpContext.AuthenticateAsync("Identity.Application");
                    if (!result.Succeeded)
                    {
                        throw new WebApiApplicationException(StatusCodes.Status412PreconditionFailed,
                            ErrorMessages.InvalidApiKey);
                    }
                }

                //if (!OpenUrls.Urls.Any(x => httpContext.Request.Path.StartsWithSegments(x)) &&
                //    !OpenUrls.WebUrls.Any(x => httpContext.Request.Path.StartsWithSegments(x)) &&
                //    httpContext.Request.Method != "OPTIONS")
                //{
                //    //Handle Invalid Api Key
                //    if (!(httpContext.Request.Headers.TryGetValue(HttpRequestHeaders.ApiKey,
                //              out StringValues apiKeyValues) &&
                //          (apiKeyValues.FirstOrDefault()?.Equals(HttpRequestHeaders.ApiKeyValue) ?? false)))
                //        throw new WebApiApplicationException(StatusCodes.Status412PreconditionFailed, ErrorMessages.InvalidApiKey);
                //}

                await _next(httpContext);

                if (!OpenUrls.WebUrls.Any(x => httpContext.Request.Path.StartsWithSegments(x)))
                {
                    switch (httpContext.Response.StatusCode)
                    {
                        //Handle Unauthorized Error
                        case StatusCodes.Status401Unauthorized:
                            await HandleUnauthorizedAsync(httpContext);
                            break;
                        //Handle NotFound Error
                        case StatusCodes.Status404NotFound:
                            await HandleNotFoundAsync(httpContext);
                            break;
                    }
                }
            }
            catch (ModelValidationException ex)
            {
                _logger.LogError("{@Exception}", ex);
                await HandleModelValidationExceptionAsync(httpContext, ex);
            }
            catch (WebApiApplicationException ex)
            {
                _logger.LogError("{@Exception}", ex);
                await HandleApiAppExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
                {
                _logger.LogCritical("{@Exception}", ex);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            //return context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse(false,
            //    "Internal Server Error. Please Contact your Administrator.", exception)));
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse(false,
                ErrorMessages.InternalServerError, null, exception.Message)));
        }

        private static Task HandleModelValidationExceptionAsync(HttpContext context, ModelValidationException resultException)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = resultException.StatusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse(false,
                resultException.ErrorMessage, null, resultException.ErrorData)));
        }

        private static Task HandleApiAppExceptionAsync(HttpContext context, WebApiApplicationException resultException)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = resultException.StatusCode;

            object exData = resultException.Data != null ? resultException.ErrorData : resultException;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse(false,
                resultException.ErrorMessage, null, exData)));
        }

        private static Task HandleUnauthorizedAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse(false,
                ErrorMessages.UnAuthorized)));
        }

        private static Task HandleNotFoundAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse(false,
                ErrorMessages.Page404)));
        }

        //private static HttpContext AddHeaders(HttpContext context)
        //{
        //    //context.Response.Headers.Add("access-control-allow-credentials", "true");
        //    //context.Response.Headers.Add("access-control-allow-headers", "*");
        //    //context.Response.Headers.Add("access-control-allow-origin", "*");
        //    //context.Response.Headers.Add("date", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
        //    //context.Response.Headers.Add("server", "Kestrel");
        //    //context.Response.Headers.Add("x-powered-by", "ASP.NET");

        //    return context;
        //}
    }
}
