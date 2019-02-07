using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Server.AppStart;
using WebAPI_ViewModel.Response;

namespace WebAPI_Server.Controllers
{
    /// <inheritdoc />
    [Authorize]
    [AccessTokenFilter]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        protected OkObjectResult Ok(object value, string msg)
        {
            return base.Ok(new ApiResponse(true, msg, value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="msg"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        protected virtual ObjectResult StatusCode(int statusCode, object value, string msg)
        {
            return base.StatusCode(statusCode, new ApiResponse(false, msg, value));
        }

        /// <inheritdoc />
        [ApiExplorerSettings(IgnoreApi = true)]
        public override BadRequestObjectResult BadRequest(object error)
        {
            return base.BadRequest(new ApiResponse(false, error.ToString(), null));
        }
    }
}