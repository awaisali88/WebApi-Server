using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories.Attributes;

namespace WebAPI_Model.Test
{
    [Table("Phones", Schema = "DAB")]
    public class Phone
    {
        [Key]
        [Identity]
        public int Id { get; set; }

        public string Number { get; set; }

        public bool IsActive { get; set; }

        [IgnoreUpdate]
        public string Code { get; set; }
    }
}