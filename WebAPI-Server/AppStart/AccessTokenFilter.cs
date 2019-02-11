using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

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

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    //Log.Information("Executing Action in Controller with Data");
        //    base.OnActionExecuting(context);
        //}
    }

    internal class AllowNoAccessToken : ActionFilterAttribute
    {
    }

}
