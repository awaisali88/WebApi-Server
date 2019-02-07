﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebAPI_Server.Middleware
{
#pragma warning disable 1591
    public class ProtectFolder
    {
        private readonly RequestDelegate _next;
        private readonly PathString _path;
        private readonly string _policyName;

        public ProtectFolder(RequestDelegate next, ProtectFolderOptions options)
        {
            _next = next;
            _path = options.Path;
            _policyName = options.PolicyName;
        }

        public async Task Invoke(HttpContext httpContext,
            IAuthorizationService authorizationService)
        {
            if (httpContext.Request.Path.StartsWithSegments(_path))
            {
                var authorized = await authorizationService.AuthorizeAsync(
                    httpContext.User, null, _policyName);
                if (!authorized.Succeeded)
                {
                    await httpContext.Authentication.ChallengeAsync();
                    return;
                }
            }

            await _next(httpContext);
        }
    }

    public class ProtectFolderOptions
    {
        public PathString Path { get; set; }
        public string PolicyName { get; set; }
    }

    public static class ProtectFolderExtensions
    {
        public static IApplicationBuilder UseProtectFolder(
            this IApplicationBuilder builder,
            ProtectFolderOptions options)
        {
            return builder.UseMiddleware<ProtectFolder>(options);
        }
    }
#pragma warning restore 1591
}
