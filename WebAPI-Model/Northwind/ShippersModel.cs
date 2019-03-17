using Dapper.Repositories.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories;

namespace WebAPI_Model
{
    [Table("Shippers", Schema = "dbo")]
    public class ShippersModel : DefaultColumns  
    {
        [Identity, Key]  
        public int ShipperID { get; set; }  
  
        public string CompanyName { get; set; }  
  
        public string Phone { get; set; }  
  
    }
}
