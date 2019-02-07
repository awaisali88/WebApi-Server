using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories;

namespace Dapper.Identity.Stores
{
    [Table("AppUserLogin")]
    public class ApplicationUserLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public string UserId { get; set; }
    }

    [Table("AppUserToken")]
    public class ApplicationUserToken
    {
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    [Table("AppUserClaim")]
    public class ApplicationUserClaim
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    [Table("AppUserRole")]
    public class ApplicationUserRole : DefaultColumns
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }

    [Table("AppRoleClaim")]
    public class ApplicationRoleClaim
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
