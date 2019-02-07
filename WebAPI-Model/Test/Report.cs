using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories.Attributes;
using Dapper.Repositories.Attributes.Joins;

namespace WebAPI_Model.Test
{
    [Table("Reports")]
    public  class Report
    {
        [Key]
        [IgnoreUpdate]
        public int Id { get; set; }

        [Key]
        public int AnotherId { get; set; }

        public int UserId { get; set; }

        [LeftJoin("Users", "UserId", "Id")]
        public User User { get; set; }
    }
}
