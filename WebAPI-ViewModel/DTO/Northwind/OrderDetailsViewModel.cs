using System;

namespace WebAPI_ViewModel.DTO
{
    public class OrderDetailsViewModel : DefaultViewModel  
    {  
        public int OrderID { get; set; }  
  
        public int ProductID { get; set; }  
  
        public decimal UnitPrice { get; set; }  
  
        public short Quantity { get; set; }  
  
        public double Discount { get; set; }

        public OrdersViewModel Orders { get; set; }

        public ProductsViewModel Products { get; set; }
    }
}
