using Dapper.Repositories.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories;
using Dapper.Repositories.Attributes.Joins;

namespace WebAPI_Model
{
    [Table("Orders", Schema = "dbo")]
    public class OrdersModel : DefaultColumns  
    {
        [Identity, Key]  
        public int OrderID { get; set; }  
  
        public string CustomerID { get; set; }  
  
        public int? EmployeeID { get; set; }  
  
        public DateTime? OrderDate { get; set; }  
  
        public DateTime? RequiredDate { get; set; }  
  
        public DateTime? ShippedDate { get; set; }  
  
        public int? ShipVia { get; set; }  
  
        public decimal? Freight { get; set; }  
  
        public string ShipName { get; set; }  
  
        public string ShipAddress { get; set; }  
  
        public string ShipCity { get; set; }  
  
        public string ShipRegion { get; set; }  
  
        public string ShipPostalCode { get; set; }  
  
        public string ShipCountry { get; set; }

        [InnerJoin("Customers", nameof(CustomerID), nameof(CustomersModel.CustomerID), "dbo")]
        public CustomersModel Customers { get; set; }

        [InnerJoin("Employees", nameof(EmployeeID), nameof(EmployeesModel.EmployeeID), "dbo")]
        public EmployeesModel Employees { get; set; }

        [InnerJoin("Shippers", nameof(ShipVia), nameof(ShippersModel.ShipperID), "dbo")]
        public ShippersModel Shippers { get; set; }

        [LeftJoin("OrderDetails", nameof(OrderID), nameof(OrderDetailsModel.OrderID), "dbo")]
        public List<OrderDetailsModel> OrderDetails { get; set; }

    }
}
