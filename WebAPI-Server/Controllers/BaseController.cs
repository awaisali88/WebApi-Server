using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        protected virtual ObjectResult StatusCodeResult(int statusCode, object value, string msg)
        {
            return base.StatusCode(statusCode, JsonConvert.SerializeObject(new ApiResponse(false, msg, value)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public BadRequestObjectResult BadRequest(string msg)
        {
            return base.BadRequest(new ApiResponse(false, msg, null));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public BadRequestObjectResult BadRequest(string msg, object data)
        {
            return base.BadRequest(new ApiResponse(false, msg, data));
        }
    }
}