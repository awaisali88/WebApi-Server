using Dapper.Repositories.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories;

namespace WebAPI_Model
{
    [Table("TestRepo", Schema = "dbo")]
    public class TestRepoModel : DefaultColumns
	{
        [Identity, Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public int? CheckAddC { get; set; }
    }
}
