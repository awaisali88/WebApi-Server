using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories.Attributes;
using Dapper.Repositories.Attributes.Joins;
using Dapper.Repositories.Attributes.LogicalDelete;

namespace WebAPI_Model.Test
{
    [Table("Cars")]
    public class Car
    {
        [Key]
        [Identity]
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }

        public int UserId { get; set; }

        [LeftJoin("Users", "UserId", "Id")]
        public User User { get; set; }

        [Status]
        public StatusCar Status { get; set; }
    }

    public enum StatusCar
    {
        Inactive = 0,

        Active = 1,

        [Deleted]
        Deleted = -1
    }
}