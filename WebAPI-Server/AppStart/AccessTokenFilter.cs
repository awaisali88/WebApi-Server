using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI_Server.AppStart
{
    internal class AccessTokenFilter : ActionFilterAttribute
    {
        internal bool UseAccessToken { get; set; }

        internal string Policy { get; set; }

        internal AccessTokenFilter(bool useAccessToken = true)
        {
            UseAccessToken = useAccessToken;

            Policy = useAccessToken ? "All" : "None";
        }
    }

    internal class AllowNoAccessToken : ActionFilterAttribute
    {
    }

}
