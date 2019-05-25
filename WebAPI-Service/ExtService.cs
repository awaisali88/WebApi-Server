using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;

namespace WebAPI_Service
{
    public static class ExtService
    {
        public static T JwtClaimValue<T>(this ClaimsPrincipal claim, string value)
        {
            string data = claim != null && claim.Claims.Any()
                ? claim.Claims.Single(c => c.Type.ToLower() == "data".ToLower()).Value
                : "";

            if (!string.IsNullOrEmpty(data))
            {
                string val = Encoding.ASCII.GetString(Convert.FromBase64String(data));
                object jwtData = JsonConvert.DeserializeObject<object>(val);
                var returnValue = typeof(object).GetProperty(value).GetValue(jwtData);
                return (T) Convert.ChangeType(returnValue, typeof(T));
            }

            return default(T);
        }
    }

    public static class JwtClaims
    {
        public const string UserId = "Id";
    }
}

