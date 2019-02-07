using System.Collections.Generic;
using Dapper.Identity.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace WebAPI_BAL.IdentityManager
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(RoleStore roleStore, IEnumerable<IRoleValidator<ApplicationRole>> roleValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<ApplicationRoleManager> logger)
            : base(roleStore, roleValidators, keyNormalizer, errors, logger)
        {
        }
        //public static ApplicationRoleManager Create(
        //IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        //{
        //return new ApplicationRoleManager(
        //new ApplicationRoleStore(context.Get<Identity_DbContext>()));
        //}
    }
}
