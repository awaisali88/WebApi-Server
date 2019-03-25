using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_Server.Controllers
{
    /// <inheritdoc />
    [Route("api/co")]
    [ApiController]
    public class CommonController : BaseController
    {
        private readonly IAntiforgery _antiForgery;

        /// <inheritdoc />
        public CommonController(IAntiforgery antiForgery)
        {
            _antiForgery = antiForgery;
        }

        /// <summary>
        /// Get AntiForgery token for Form posting
        /// </summary>
        /// <returns></returns>
        [HttpGet("aft")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public IActionResult GetAntiForgeryToken()
        {
            var tokens = _antiForgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("XSRF-REQUEST-TOKEN", tokens.RequestToken, new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = false,
                Secure = true
            });
            return NoContent();

            //return Ok(new
            //{
            //    token = tokens.RequestToken,
            //    tokenName = tokens.HeaderName
            //}, InfoMessages.CommonInfoMessage);
        }
    }
}