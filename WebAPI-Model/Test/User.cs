using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories.Attributes;
using Dapper.Repositories.Attributes.Joins;
using Dapper.Repositories.Attributes.LogicalDelete;

namespace WebAPI_Model.Test
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Identity]
        public int Id { get; set; }

        public string ReadOnly => "test";

        [Column(Order = 1)]
        public string Name { get; set; }

        public int AddressId { get; set; }

        public int PhoneId { get; set; }

        public int OfficePhoneId { get; set; }

        [LeftJoin("Cars", "Id", "UserId")]
        public List<Car> Cars { get; set; }

        [LeftJoin("Addresses", "AddressId", "Id")]
        public Address Addresses { get; set; }

        [InnerJoin("Phones", "PhoneId", "Id", "DAB")]
        public Phone Phone { get; set; }

        [InnerJoin("Phones", "OfficePhoneId", "Id", "DAB")]
        public Phone OfficePhone { get; set; }

        [Status]
        [Deleted]
        public bool Deleted { get; set; }

        [UpdatedAt]
        public DateTime? UpdatedAt { get; set; }
    }
}