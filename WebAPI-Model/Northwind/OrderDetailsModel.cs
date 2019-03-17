using Dapper.Repositories.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories;
using Dapper.Repositories.Attributes.Joins;

namespace WebAPI_Model
{
    [Table("OrderDetails", Schema = "dbo")]
    public class OrderDetailsModel : DefaultColumns  
    {
        [Identity, Key]  
        public int OrderID { get; set; }  

        [Identity, Key]  
        public int ProductID { get; set; }  
  
        public decimal UnitPrice { get; set; }  
  
        public short Quantity { get; set; }  
  
        public double Discount { get; set; }

        [InnerJoin("Orders", nameof(OrderID), nameof(OrdersModel.OrderID), "dbo")]
        public OrdersModel Orders { get; set; }

        [InnerJoin("Products", nameof(ProductID), nameof(ProductsModel.ProductID), "dbo")]
        public ProductsModel Products { get; set; }
    }
}
