using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Common;
using Common.Exception;
using Common.Messages;
using Dapper.Identity.Stores;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebAPI_BAL.IdentityManager;
using WebAPI_BAL.JwtGenerator;
using WebAPI_BAL.NotificationManager;
using WebAPI_ViewModel.Identity;
//using Microsoft.EntityFrameworkCore;

namespace WebAPI_BAL.AuthLogic
{
    public interface IAuthentication
    {
        Task<bool> RegisterUserAsync(RegisterUserViewModel data);
        Task<bool> AddNewUserAsync(ClaimsPrincipal claim, AddNewUserViewModel data);
        Task<bool> DeleteUserAsync(ClaimsPrincipal claim, string userId);
        Task<JwtToken> AuthenticateUserAsync(LoginUserViewModel data);
        Task<JwtToken> ExternalAuthenticationAsync(RegisterUserViewModel data);
        Task<bool> SetPasswordAsync(ClaimsPrincipal claim, SetPasswordViewModel data);
        Task<bool> GeneratePasswordResetTokenAsync(string userEmail);
        Task<bool> ChangePasswordAsync(ClaimsPrincipal claim, ChangePasswordViewModel data);
        Task<bool> VerifyEmailTokenAsync(string token, string userId);
        Task<bool> VerifyPasswordResetTokenAsync(string token, string userId);
        Task<bool> ResetPasswordAsync(ResetPasswordWithTokenViewModel data);
        Task<JwtToken> RefreshTokenAsync(string token, string userId, string tokenNumber);
        Task<bool> RevokeToken(ClaimsPrincipal claim);

        Task<IdentityResult> ConfirmEmail(string token, string userId);
    }

    public class Authentication : IAuthentication
    {
        private readonly ApplicationUserManager _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationSignInManager _signInManager;
        //private readonly UserStore _userStore;
        private readonly ITokenManager _tokenManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<Authentication> _logger;

        public Authentication(ApplicationUserManager userManager, IMapper mapper,
            ApplicationSignInManager signInManager, IEmailSender emailSender, ISmsSender smsSender, 
            IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions,
            IHostingEnvironment env, IHttpContextAccessor httpContextAccessor, ITokenManager tokenManager, ILogger<Authentication> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _tokenManager = tokenManager;
            _logger = logger;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserViewModel data)
        {

            ApplicationUser user = _mapper.Map<ApplicationUser>(data);
            bool userAlreadyExist = _userManager.Users.Any(x => x.UserName == user.UserName); 
            if (userAlreadyExist)
                throw new WebApiApplicationException(StatusCodes.Status409Conflict, ErrorMessages.ErrorCreatingLocalUser,
                    new object[]
                    {
                        new
                        {
                            Code = ErrorMessages.UserExistCode,
                            Description = ErrorMessages.UserExistDescription.Replace("[USERNAME]", user.UserName)
                        }
                    });

            var result = await _userManager.CreateAsync(user, data.Password);
            if (!result.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status409Conflict, ErrorMessages.ErrorCreatingLocalUser, result.Errors.ToList());

            await SendConfirmationEmail(user);

            return true;
        }

        public async Task<bool> AddNewUserAsync(ClaimsPrincipal claim, AddNewUserViewModel data)
        {
            string userId = ExtBusinessLogic.UserValue(claim);
            data.CreatedBy = userId;
            ApplicationUser user = _mapper.Map<ApplicationUser>(data);
            bool userAlreadyExit = _userManager.Users.Any(x => x.UserName == user.UserName);
            if (userAlreadyExit)
                throw new WebApiApplicationException(StatusCodes.Status409Conflict, ErrorMessages.ErrorCreatingLocalUser,
                    new object[]
                    {
                        new
                        {
                            Code = ErrorMessages.UserExistCode,
                            Description = ErrorMessages.UserExistDescription.Replace("[USERNAME]", user.UserName)
                        }
                    });

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status409Conflict, ErrorMessages.ErrorCreatingLocalUser, result.Errors.ToList());

            await SendConfirmationEmail(user);

            return true;
        }

        public async Task<bool> DeleteUserAsync(ClaimsPrincipal claim, string userId)
        {
            ApplicationUser data = await _userManager.FindByIdAsync(userId);
            if (data == null)
                throw new WebApiApplicationException(StatusCodes.Status404NotFound, ErrorMessages.ErrorUserNotFound);

            //CheckRecord(data);

            var result = await _userManager.RemovePasswordAsync(data);
            if (!result.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status409Conflict, ErrorMessages.ErrorCreatingLocalUser, result.Errors.ToList());

            var deleteResult = await _userManager.DeleteAsync(data);
            if (!deleteResult.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status409Conflict, ErrorMessages.ErrorCreatingLocalUser, result.Errors.ToList());

            return true;
        }

        public async Task<JwtToken> AuthenticateUserAsync(LoginUserViewModel data)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(data.Email, data.Password, false, false);
            if (!signInResult.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status403Forbidden, ErrorMessages.InvalidUser,
                    new
                    {
                        signInResult.IsNotAllowed,
                        signInResult.IsLockedOut,
                        signInResult.RequiresTwoFactor,
                        msg = signInResult.IsNotAllowed ? ErrorMessages.UserNotAllowed : signInResult.IsLockedOut ? ErrorMessages.UserLockedOut : 
                            signInResult.RequiresTwoFactor ? ErrorMessages.RequireTwoFactor : string.Empty
                    });

            var identity = await GetClaimsIdentity(data.Email, data.Password);
            if (identity == null)
                throw new WebApiApplicationException(StatusCodes.Status403Forbidden, ErrorMessages.InvalidUser);

            //var jsonSerlizeSettings = new JsonSerializerSettings {Formatting = Formatting.Indented};
            JwtToken jwt = await Tokens.GenerateJwt(identity, _jwtFactory, data.Email, _jwtOptions,
                _httpContextAccessor.HttpContext.Request.Headers[HttpRequestHeaders.RequestUrl]);

            ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.UserName == data.Email);
            if (user != null)
            {
                user.TokenNumber = jwt.refresh_token;
                await _userManager.UpdateAsync(user);
            }

            //_userStore.Context.SaveChanges();

            return jwt;
            //return JsonConvert.SerializeObject(jwt, jsonSerlizeSettings);
        }

        public async Task<JwtToken> ExternalAuthenticationAsync(RegisterUserViewModel data)
        {
            // 4. ready to create the local user account (if necessary) and jwt
            var user = await _userManager.FindByEmailAsync(data.Email);

            if (user == null)
            {
                ApplicationUser appUser = _mapper.Map<ApplicationUser>(data);

                var result = await _userManager.CreateAsync(appUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

                if (!result.Succeeded)
                    throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.ErrorCreatingFbUser, result.Errors.ToList());
                //if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            }

            // generate the jwt for the local user...
            var localUser = await _userManager.FindByNameAsync(data.Email);

            if (localUser == null)
            {
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.ErrorCreatingLocalUser);
                //return BadRequest(Errors.AddErrorToModelState("login_failure", "Failed to create local user account.", ModelState));
            }

            //var jsonSerlizeSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            var jwt = await Tokens.GenerateJwt(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id, Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress)),
                _jwtFactory, localUser.UserName, _jwtOptions, _httpContextAccessor.HttpContext.Request.Headers[HttpRequestHeaders.RequestUrl]);

            ApplicationUser userUpdate = _userManager.Users.FirstOrDefault(x => x.UserName == data.Email);
            if (userUpdate != null) userUpdate.TokenNumber = jwt.refresh_token;
            await _userManager.UpdateAsync(user);
            //_userStore.Context.SaveChanges();

            return jwt;
        }

        public async Task<bool> SetPasswordAsync(ClaimsPrincipal claim, SetPasswordViewModel data)
        {
            ApplicationUser user =
                _userManager.Users.FirstOrDefault(x => x.Id == ExtBusinessLogic.UserValue(claim, nameof(ApplicationUser.Id)));
            ExtBusinessLogic.CheckRecord(user);

            IdentityResult removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.ErrorSetPassword, removePasswordResult.Errors.ToList());

            IdentityResult result = await _userManager.AddPasswordAsync(user, data.NewPassword);
            if (!result.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.ErrorSetPassword, result.Errors.ToList());

            return true;
        }

        public async Task<bool> GeneratePasswordResetTokenAsync(string userEmail)
        {
            ApplicationUser user =
                _userManager.Users.FirstOrDefault(x => x.UserName == userEmail);

            ExtBusinessLogic.CheckRecord(user);

            if (user != null && !user.EmailConfirmed)
                throw new WebApiApplicationException(StatusCodes.Status403Forbidden, ErrorMessages.EmailNotVerified);

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordReset = System.IO.Path.Combine(_env.WebRootPath, EmailFiles.ResetPassword);
            string passwordResetContent = System.IO.File.ReadAllText(passwordReset);

            string callbackUrl = _httpContextAccessor.HttpContext.Request.Headers[HttpRequestHeaders.RequestUrl]
                .ToString().CreateRequestUrl(confirmationToken: HttpUtility.UrlEncode(passwordResetToken),
                    // ReSharper disable once PossibleNullReferenceException
                    userId: user.Id);

            passwordResetContent = passwordResetContent.Replace(EmailContentKeywords.ActivateLink, callbackUrl);
            await _emailSender.SendEmailAsync(user.Email, EmailSubject.ResetPassword, passwordResetContent);
            return true;
        }

        public async Task<bool> VerifyPasswordResetTokenAsync(string token, string userId)
        {
            ApplicationUser user =
                _userManager.Users.FirstOrDefault(x => x.Id == userId);

            ExtBusinessLogic.CheckRecord(user);

            return await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", token);
        }

        public async Task<bool> ChangePasswordAsync(ClaimsPrincipal claim, ChangePasswordViewModel data)
        {
            ApplicationUser user =
                _userManager.Users.FirstOrDefault(
                    x => x.Id == ExtBusinessLogic.UserValue(claim, nameof(ApplicationUser.Id)) && x.Status && !x.Trashed);
            IdentityResult result = await _userManager.ChangePasswordAsync(user, data.OldPassword, data.NewPassword);
            if (!result.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.ErrorSetPassword, result.Errors.ToList());

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordWithTokenViewModel data)
        {
            ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.Id == data.UserId);

            ExtBusinessLogic.CheckRecord(user);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, data.Token, data.ConfirmPassword);
            if (!result.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status400BadRequest, ErrorMessages.ErrorSetPassword, result.Errors.ToList());

            return true;
        }

        public async Task<bool> VerifyEmailTokenAsync(string token, string userId)
        {
            //var handler = new JwtSecurityTokenHandler();
            //var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            //string userId = jsonToken?.Claims.First(claim => claim.Type == "id").Value;

            ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                throw new WebApiApplicationException(StatusCodes.Status404NotFound, ErrorMessages.ErrorUserNotFound);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                throw new WebApiApplicationException(StatusCodes.Status409Conflict, ErrorMessages.InvalidUser, result.Errors.ToList());

            if (!user.Status || user.Trashed)
            {
                user.Status = true;
                user.Trashed = false;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                    throw new WebApiApplicationException(StatusCodes.Status409Conflict, ErrorMessages.InvalidUser, result.Errors.ToList());
            }
            if (!await _userManager.HasPasswordAsync(user))
            {
                return await GeneratePasswordResetTokenAsync(user.Email);
            }

            return true;
        }

        public async Task<JwtToken> RefreshTokenAsync(string token, string userId, string tokenNumber)
        {
            ApplicationUser user =
                _userManager.Users.FirstOrDefault(x => x.Id == userId && x.Status && !x.Trashed);
            if (!(user != null && user.TokenNumber == tokenNumber))
                throw new WebApiApplicationException(StatusCodes.Status404NotFound, ErrorMessages.ErrorUserNotFound);

            var identity = await GetClaimsIdentity(user.Email);
            //var jsonSerlizeSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, user.Email, _jwtOptions, _httpContextAccessor.HttpContext.Request.Headers[HttpRequestHeaders.RequestUrl]);

            user.TokenNumber = jwt.refresh_token;
            await _userManager.UpdateAsync(user);
            //_userStore.Context.SaveChanges();

            return jwt;
        }

        public async Task<bool> RevokeToken(ClaimsPrincipal claim)
        {
            ApplicationUser user = _userManager.Users.FirstOrDefault(x =>
                x.Id == ExtBusinessLogic.UserValue(claim, nameof(ApplicationUser.Id)));
            if (user == null)
                throw new WebApiApplicationException(StatusCodes.Status404NotFound, ErrorMessages.ErrorUserNotFound);

            //CheckRecord(user);

            user.TokenNumber = null;
            await _userManager.UpdateAsync(user);
            //_userStore.Context.SaveChanges();

            await _tokenManager.DeactivateCurrentAsync();

            return true;
        }

        #region MVC View Methods
        public async Task<IdentityResult> ConfirmEmail(string token, string userId)
        {
            //var handler = new JwtSecurityTokenHandler();
            //var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            //string userId = jsonToken?.Claims.First(claim => claim.Type == "id").Value;

            ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                throw new WebApiApplicationException(StatusCodes.Status404NotFound, ErrorMessages.ErrorUserNotFound);

            return await _userManager.ConfirmEmailAsync(user, token);
        }
        #endregion

        #region Private Methods
        private async Task SendConfirmationEmail(ApplicationUser user)
        {
            //Generate email for confirmation
            string emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var accountActivationEmail = System.IO.Path.Combine(_env.WebRootPath, EmailFiles.ConfirmEmail);
            string accountActivationEmailContent = System.IO.File.ReadAllText(accountActivationEmail);

            //string url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}";
            //string callBackUrl = $"{url}/auth/activation?c={HttpUtility.UrlEncode(emailConfirmationToken)}&f={user.Id}";

            string callbackUrl = _httpContextAccessor.HttpContext.Request.Headers[HttpRequestHeaders.RequestUrl]
                .ToString().CreateRequestUrl(confirmationToken: HttpUtility.UrlEncode(emailConfirmationToken),
                    userId: user.Id);
            accountActivationEmailContent = accountActivationEmailContent.Replace(EmailContentKeywords.ActivateLink, callbackUrl);

            await _emailSender.SendEmailAsync(user.Email, EmailSubject.ConfirmEmail, accountActivationEmailContent);

            //Task emailTask = Task.Run(async () =>
            //{
            //    await _emailSender.SendEmailAsync(user.Email, EmailSubject.ConfirmEmail,
            //        accountActivationEmailContent);
            //});
            //
            //return await emailTask.ConfigureAwait(false);
        }
        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);
            if (!userToVerify.Status) return await Task.FromResult<ClaimsIdentity>(null);

            bool passwordConfirmed = await _userManager.CheckPasswordAsync(userToVerify, password);
            if (!passwordConfirmed)
                throw new WebApiApplicationException(StatusCodes.Status401Unauthorized, ErrorMessages.InvalidUser);

            bool emailConfirmed = await _userManager.IsEmailConfirmedAsync(userToVerify);
            if (!emailConfirmed)
                throw new WebApiApplicationException(StatusCodes.Status403Forbidden, ErrorMessages.EmailNotVerified);

            return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id, Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress)));
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName)
        {
            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            bool emailConfirmed = await _userManager.IsEmailConfirmedAsync(userToVerify);
            if (!emailConfirmed)
                throw new WebApiApplicationException(StatusCodes.Status403Forbidden, ErrorMessages.EmailNotVerified);

            return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id, Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress)));
        }

    #endregion

  }
}
