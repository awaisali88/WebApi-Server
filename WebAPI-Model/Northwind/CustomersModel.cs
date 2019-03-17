using Dapper.Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories;
using Dapper.Repositories.Attributes.Joins;

namespace WebAPI_Model
{
    [Table("Customers", Schema = "dbo")]
    public class CustomersModel : DefaultColumns  
    {
        [Identity, Key]  
        public string CustomerID { get; set; }  
  
        public string CompanyName { get; set; }  
  
        public string ContactName { get; set; }  
  
        public string ContactTitle { get; set; }  
  
        public string Address { get; set; }  
  
        public string City { get; set; }  
  
        public string Region { get; set; }  
  
        public string PostalCode { get; set; }  
  
        public string Country { get; set; }  
  
        public string Phone { get; set; }  
  
        public string Fax { get; set; }

        [LeftJoin("Orders", nameof(CustomerID), nameof(WebAPI_Model.OrdersModel.CustomerID), "dbo")]
        public List<OrdersModel> OrdersModel { get; set; }
    }
}
