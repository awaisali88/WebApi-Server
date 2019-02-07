using System.Threading.Tasks;
using Dapper.Identity.Stores;
using Microsoft.AspNetCore.Identity;

namespace WebAPI_BAL.IdentityManager
{
    public class AppEmailTokenProvider : EmailTokenProvider<ApplicationUser>
    {
        public override Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            return base.GenerateAsync(purpose, manager, user);
        }

        public override Task<bool> ValidateAsync(string purpose, string token, UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            return base.ValidateAsync(purpose, token, manager, user);
        }
    }
}
