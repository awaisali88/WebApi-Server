using System.Security.Claims;
using System.Threading.Tasks;
using Dapper.Identity.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace WebAPI_BAL.IdentityManager
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        public AppClaimsPrincipalFactory(
            ApplicationUserManager userManager,
            ApplicationRoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
            });

            return principal;
        }
    }
}
