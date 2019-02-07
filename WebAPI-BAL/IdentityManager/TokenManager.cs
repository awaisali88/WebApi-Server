using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using WebAPI_BAL.JwtGenerator;

namespace WebAPI_BAL.IdentityManager
{
    public interface ITokenManager
    {
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);
    }

    public class TokenManager : ITokenManager
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtIssuerOptions _jwtOptions;

        public TokenManager(IDistributedCache cache,
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtIssuerOptions> jwtOptions
        )
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveAsync(GetCurrentAsync());

        public async Task DeactivateCurrentAsync()
            => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
        {
            if (await _cache.GetStringAsync(GetKey(token)) == null)
            {
                //Check token and requester ip address match
                if (!string.IsNullOrEmpty(token))
                {
                    var tokenS = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
                    if (tokenS == null)
                        return false;
                    return tokenS.Claims.First(claim => claim.Type == AppConstants.RemoteIp).Value ==
                           Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
                }

                return true;
            }

            return false;
        }

        public async Task DeactivateAsync(string token)
            => await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        (_jwtOptions.Expiration - DateTime.UtcNow).Add(TimeSpan.FromMinutes(5))
                });

        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["Authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }

        private static string GetKey(string token)
            => $"tokens:deactivated:{token}";
    }
}
