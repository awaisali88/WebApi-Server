using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Model.Test
{
    [Table("Cities")]
    public class City
    {
        public Guid Identifier { get; set; }

        public string Name { get; set; }

    }
}