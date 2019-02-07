using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Common.Exception;
using Common.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI_BAL.AuthLogic;
using WebAPI_BAL.JwtGenerator;
using WebAPI_Server.AppStart;
using WebAPI_Service.Service;
using WebAPI_ViewModel.ConfigSettings;
using WebAPI_ViewModel.Identity;
using WebAPI_ViewModel.Response;

namespace WebAPI_Server.Controllers
{
    /// <inheritdoc />
    [Route("api/auth")]
    [ApiController]
    public class AccountController : BaseController
    {
        //private readonly ClaimsPrincipal _caller;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthentication _auth;
        private readonly IAuthorization _authorize;
        private readonly FacebookAuthSettings _fbAuthSettings;
        private static readonly HttpClient Client = new HttpClient();
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthenticationService _authService;

        /// <inheritdoc />
        public AccountController(IAuthentication auth, IOptions<FacebookAuthSettings> fbAuthSettingsAccessor,
            IHttpContextAccessor httpContextAccessor, IAuthorization authorize, IAuthenticationService authService, ILogger<AccountController> logger)
        {
            _fbAuthSettings = fbAuthSettingsAccessor.Value;
            _auth = auth;
            _authorize = authorize;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
            _logger = logger;
        }

        #region Authentication

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="data">Information of new user</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [RequireCallbackUrl]
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> RegisterUser([FromBody] RegisterUserViewModel data)
        {
            var result = await _auth.RegisterUserAsync(data);

            if (result)
                return StatusCode(StatusCodes.Status201Created, null, InfoMessages.UserRegistered);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="data">Information of new user</param>
        /// <returns>New user info</returns>
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(ApiResponse<AddNewUserViewModel>))]
        [SwaggerResponse(StatusCodes.Status409Conflict, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [RequireCallbackUrl]
        [HttpPost("add-user")]
        public async Task<ActionResult<ApiResponse<AddNewUserViewModel>>> AddUser([FromBody] AddNewUserViewModel data)
        {
            var result = await _auth.AddNewUserAsync(User, data);

            if (result)
                return StatusCode(StatusCodes.Status201Created, null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Delete user from system
        /// </summary>
        /// <param name="userId">User Id to delete</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [HttpDelete("remove-user")]
        public async Task<ActionResult<ApiResponse>> RemoveUser([FromBody] string userId)
        {
            var result = await _auth.DeleteUserAsync(User, userId);

            if (result)
                return Ok(null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Create new user bae on facebook authentication token and login that user.
        /// </summary>
        /// <param name="model">Facebook access token</param>
        /// <returns>User json web token</returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse<JwtToken>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, type: typeof(ApiResponse))]
        [RequireCallbackUrl]
        [HttpPost("facebook")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<JwtToken>>> Facebook([FromBody]FacebookAuthViewModel model)
        {
            // 1.generate an app access token
            var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            // 2. validate the user access token
            var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                throw new WebApiApplicationException(StatusCodes.Status409Conflict, ErrorMessages.InvalidFbToken, MethodBase.GetCurrentMethod().GetParameters());
            }

            // 3. we've got a valid token so we can request user data from fb
            var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            var registerUser = new RegisterUserViewModel()
            {
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                FacebookId = userInfo.Id,
                Email = userInfo.Email,
                PictureUrl = userInfo.Picture.Data.Url
            };

            JwtToken result = await _auth.ExternalAuthenticationAsync(registerUser);
            if (result == null)
                throw new WebApiApplicationException(StatusCodes.Status401Unauthorized, ErrorMessages.InvalidUser);

            return Ok(result, InfoMessages.UserSignin);
        }

        /// <summary>
        /// Authenticate and sign in user
        /// </summary>
        /// <param name="data">Username and password of the user</param>
        /// <returns>JWT along with userid</returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse<JwtToken>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, type: typeof(ApiResponse))]
        [RequireCallbackUrl]
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<JwtToken>>> Authenticate([FromBody] LoginUserViewModel data)
        {
            _authService.ValidateLogin(data);

            JwtToken result = await _auth.AuthenticateUserAsync(data);
            if (result == null)
                    throw new WebApiApplicationException(StatusCodes.Status401Unauthorized, ErrorMessages.InvalidUser, MethodBase.GetCurrentMethod().GetParameters());
            //return StatusCode(StatusCodes.Status401Unauthorized, "Invalid username or password", null);

            return Ok(result, InfoMessages.UserSignin);
        }

        /// <summary>
        /// Set new password based on user logged in
        /// </summary>
        /// <param name="data">New password</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [HttpPost("set-password")]
        public async Task<ActionResult<ApiResponse>> SetPassword([FromBody] SetPasswordViewModel data)
        {
            var result = await _auth.SetPasswordAsync(User, data);

            if (result)
                return Ok(null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Change Password based on user logged in and provided with old password
        /// </summary>
        /// <param name="data">Old and new password</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [HttpPut("change-password")]
        public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordViewModel data)
        {
            var result = await _auth.ChangePasswordAsync(User, data);

            if (result)
                return Ok(null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Generate reset password token
        /// </summary>
        /// <param name="data">user email for reset password</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, type: typeof(ApiResponse))]
        [RequireCallbackUrl]
        [HttpPost("reset-password-token")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> GenerateResetPasswordToken([FromBody] PasswordResetTokenViewModel data)
        {
            var result = await _auth.GeneratePasswordResetTokenAsync(data.Email);

            if (result)
                return Ok(null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Reset password with email token and userid
        /// </summary>
        /// <param name="data">Data for token, user id and new password</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordWithTokenViewModel data)
        {
            var result = await _auth.ResetPasswordAsync(data);

            if (result)
                return Ok(null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Verify email token for registration and new user
        /// </summary>
        /// <param name="data">Token and user id to verify</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, type: typeof(ApiResponse))]
        [HttpPost("verify-email")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> VerifyEmailToken([FromBody] VerifyTokenViewModel data)
        {
            var result = await _auth.VerifyEmailTokenAsync(data.Token, data.UserId);

            if (result)
                return Ok(null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        //[HttpPost("verify-reset-password")]
        //[AllowAnonymous]
        //public async Task<ActionResult<ApiResponse>> VerifyResetPasswordToken([FromBody] VerifyTokenViewModel data)
        //{
        //    var result = await _auth.VerifyPasswordResetTokenAsync(data.Token, data.UserId);

        //    if (result)
        //        return Ok(null, InfoMessages.CommonInfoMessage);

        //    return BadRequest(ErrorMessages.CommonErrorMessage);
        //}

        /// <summary>
        /// Generate refresh token for logged in user
        /// </summary>
        /// <param name="data">JWT, refresh token and user id required</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, type: typeof(ApiResponse))]
        [RequireCallbackUrl]
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenViewModel data)
        {
            var result = await _auth.RefreshTokenAsync(data.Token, data.UserId, data.TokenNumber);

            if (result == null)
                throw new WebApiApplicationException(StatusCodes.Status401Unauthorized, ErrorMessages.InvalidUser, MethodBase.GetCurrentMethod().GetParameters());

            return Ok(result, InfoMessages.UserSignin);
        }

        /// <summary>
        /// Log out user (revoke generated JWT)
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [HttpPost("logoff")]
        public async Task<ActionResult<ApiResponse>> Revoke()
        {
            var result = await _auth.RevokeToken(User);

            if (result)
                return Ok(null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }
        #endregion

        #region Authorization
        /// <summary>
        /// Create new role 
        /// </summary>
        /// <param name="data">New role data</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [HttpPost("add-role")]
        public async Task<IActionResult> CreateRole([FromBody] ApplicationRoleViewModel data)
        {
            var result = await _authorize.CreateRole(User, data);

            if (result)
                return StatusCode(StatusCodes.Status201Created, null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Update Role
        /// </summary>
        /// <param name="data">Role data to update with role id</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateRole([FromBody] ApplicationRoleViewModel data)
        {
            var result = await _authorize.UpdateRole(User, data);

            if (result)
                return Ok(null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Delete role from system
        /// </summary>
        /// <param name="data">Role data to delete</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [HttpDelete("delete-role")]
        public async Task<IActionResult> RemoveRole([FromBody] ApplicationRoleViewModel data)
        {
            var result = await _authorize.RemoveRole(User, data);

            if (result)
                return Ok(null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Check if logged in use has specific role
        /// </summary>
        /// <param name="roleName">Role name to check for user</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [HttpGet("has-role")]
        public async Task<IActionResult> UserIsInRole([FromQuery] string roleName)
        {
            var result = await _authorize.UserIsInRole(User, roleName);

            if (result)
                return Ok(new { UserHasRole = true }, InfoMessages.UserHasRole);

            return StatusCode(StatusCodes.Status200OK, new { UserHasRole = false }, InfoMessages.UserHasNoRole);

            //return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Assign role to specific user
        /// </summary>
        /// <param name="data">role and user data</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, type: typeof(ApiResponse))]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleViewModel data)
        {
            var result = await _authorize.AssignRole(User, data.UserId, data.RoleId);

            if (result)
                return Ok(null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Remove user from role
        /// </summary>
        /// <param name="data">role and user data</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ApiResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, type: typeof(ApiResponse))]
        [HttpDelete("remove-role")]
        public async Task<IActionResult> RemoveFromRole([FromBody] AssignRoleViewModel data)
        {
            var result = await _authorize.RemoveFromRole(User, data.UserId, data.RoleId);

            if (result)
                return Ok(null, InfoMessages.CommonInfoMessage);

            return BadRequest(ErrorMessages.CommonErrorMessage);
        }

        /// <summary>
        /// Check if specific user has role
        /// </summary>
        /// <param name="data">role and user data</param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ApiResponse))]
        [HttpPost("user-role")]
        public async Task<IActionResult> SpecificUserIsInRole([FromBody] UserHasRoleViewModel data)
        {
            var result = await _authorize.UserIsInRole(data.UserId, data.RoleId);

            if (result)
                return Ok(new { UserHasRole = true}, InfoMessages.UserHasRole);

            return StatusCode(StatusCodes.Status200OK, new { UserHasRole = false }, InfoMessages.UserHasNoRole);
            //return BadRequest(ErrorMessages.CommonErrorMessage);
        }
        #endregion
    }
}