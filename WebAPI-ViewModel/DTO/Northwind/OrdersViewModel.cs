using System;
using System.Collections.Generic;

namespace WebAPI_ViewModel.DTO
{
    public class OrdersViewModel : DefaultViewModel  
    {  
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

        public CustomersViewModel Customers { get; set; }

        public EmployeesViewModel Employees { get; set; }

        public ShippersViewModel Shippers { get; set; }

        public List<OrderDetailsViewModel> OrderDetails { get; set; }
    }
}
