using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Dapper.Repositories;
using Dapper.Repositories.Attributes;
using Microsoft.AspNetCore.Identity;

namespace Dapper.Identity.Stores
{
    [Table("AppUsers")]
    public class ApplicationUser : DefaultColumns
    {
        public ApplicationUser() 
        {
            Id = Guid.NewGuid().ToString("N");
            Claims = new List<Claim>();
            Roles = new List<ApplicationUserRole>();
            Logins = new List<UserLoginInfo>();
            Tokens = new List<ApplicationUserToken>();
        }

        [Key, Identity]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public List<Claim> Claims { get; set; }
        public List<ApplicationUserRole> Roles { get; set; }
        public List<UserLoginInfo> Logins { get; set; }
        public List<ApplicationUserToken> Tokens { get; set; }

        public string PictureUrl { get; set; }
        public string TokenNumber { get; set; }
    }

}
