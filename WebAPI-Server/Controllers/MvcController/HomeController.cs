using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI_BAL.IdentityManager;
using WebAPI_BAL.JwtGenerator;
using WebAPI_Server.Controllers.v1;
using WebAPI_ViewModel.Identity;
using IAuthenticationService = WebAPI_Service.Service.IAuthenticationService;

namespace WebAPI_Server.Controllers.MvcController
{
#pragma warning disable 1591
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IJwtFactory _jwtFactory;
        private readonly IAuthenticationService _authService;

        public HomeController(ApplicationUserManager userManager, IMapper mapper,
            ApplicationSignInManager signInManager, ILogger<AccountController> logger, IJwtFactory jwtFactory, IAuthenticationService authService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _logger = logger;
            _jwtFactory = jwtFactory;
            _authService = authService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("")]
        [Route("Home")]
        [Route("Home/Login")]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("Home/Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            List<ErrorsModelException> errors = _authService.ValidateLoginForWeb(loginUser);
            if (!errors.Any())
            {
                var signInResult =
                    await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, true, false);
                if (signInResult.Succeeded)
                {
                    return Redirect("/api-doc");
                }

                if (signInResult.IsLockedOut)
                    ModelState.AddModelError("LockedOut", "You are locked out from system. Please Contact Administrator");
            
                if (signInResult.IsNotAllowed)
                    ModelState.AddModelError("NotAllowed", "You are not allowed to log in. Please Contact Administrator");
            
                if (signInResult.RequiresTwoFactor)
                    ModelState.AddModelError("RequireTwoFactor", "Require two factor authentication. Please Contact Administrator");
            
                ModelState.AddModelError("Error", "Invalid username and/or password.");
                return View();
            }

            return View();
        }
    }
#pragma warning restore 1591
}