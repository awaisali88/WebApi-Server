using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace WebAPI_BAL.JwtGenerator
{
    public class Tokens
    {
        public static async Task<JwtToken> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, string audience)
        {
            string tokenNumber = jwtFactory.GenerateRefreshToken();
            JwtToken response = new JwtToken
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity, audience),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds,
                refresh_token = tokenNumber
            };
            return response;
            //return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }

    public class JwtToken
    {
        public string id { get; set; }
        public string auth_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
    }

    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }
        }
    }

}
