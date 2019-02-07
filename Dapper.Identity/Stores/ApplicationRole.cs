using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Dapper.Repositories;
using Dapper.Repositories.Attributes;

namespace Dapper.Identity.Stores
{
    [Table("AppRoles")]
    public class ApplicationRole : DefaultColumns
    {
        public ApplicationRole()
        {
            GenerateNewId();
            Claims = new List<Claim>();
        }

        public void GenerateNewId()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        //public ApplicationRole(string name)
        //    : this()
        //{
        //    Name = name;
        //}
        //public ApplicationRole(string name, string description)
        //    : this(name)
        //{
        //    Description = description;
        //}

        [Key, Identity]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public List<Claim> Claims { get; set; }

    }
}
