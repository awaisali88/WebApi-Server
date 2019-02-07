using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories.Attributes;
using Dapper.Repositories.Attributes.Joins;

namespace WebAPI_Model.Test
{
    [Table("Addresses")]
    public class Address
    {
        [Key]
        [Identity]
        public int Id { get; set; }

        public string Street { get; set; }

        [LeftJoin(nameof(Users), nameof(Id), nameof(User.AddressId))]
        public List<User> Users { get; set; }

        public string CityId { get; set; }

        [InnerJoin(nameof(Cities), nameof(CityId), nameof(Test.City.Identifier))]
        public City Cities { get; set; }

        //[InnerJoin("Cities", "CityId", "Identifier")]
        //public City City { get; set; }

    }
}