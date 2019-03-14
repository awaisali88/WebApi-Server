using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories;
using Dapper.Repositories.Attributes;

namespace WebAPI_Model.Test
{
    [Table("TestRepo")]
    public class TestRepo : DefaultColumns
    {
        [Key, Identity()]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        [NotMapped]
        public override byte[] RowVersion { get => base.RowVersion; set => base.RowVersion = value; }
    }
}
