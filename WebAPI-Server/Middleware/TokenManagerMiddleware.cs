using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebAPI_BAL.IdentityManager;

namespace WebAPI_Server.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenManagerMiddleware : IMiddleware
    {
        private readonly ITokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenManager"></param>
        public TokenManagerMiddleware(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        /// <inheritdoc />
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _tokenManager.IsCurrentActiveToken())
            {
                await next(context);

                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
