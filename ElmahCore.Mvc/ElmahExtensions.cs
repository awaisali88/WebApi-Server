using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace ElmahCore.Mvc
{
    public static class ElmahExtensions
    {
        public static void RiseError(this HttpContext ctx, Exception ex)
        {
            var middleware = ctx.RequestServices.GetService<ErrorLogMiddleware>();
            middleware?.LogException(ex,ctx);
        }
    }
}