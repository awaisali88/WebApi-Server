using Dapper.Repositories.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories;
using Dapper.Repositories.Attributes.Joins;

namespace WebAPI_Model
{
    [Table("Products", Schema = "dbo")]
    public class ProductsModel : DefaultColumns  
    {
        [Identity, Key]  
        public int ProductID { get; set; }  
  
        public string ProductName { get; set; }  
  
        public int? SupplierID { get; set; }  
  
        public int? CategoryID { get; set; }  
  
        public string QuantityPerUnit { get; set; }  
  
        public decimal? UnitPrice { get; set; }  
  
        public short? UnitsInStock { get; set; }  
  
        public short? UnitsOnOrder { get; set; }  
  
        public short? ReorderLevel { get; set; }  
  
        public bool Discontinued { get; set; }

        [InnerJoin("Categories", nameof(CategoryID), nameof(CategoriesModel.CategoryID), "dbo")]
        public CategoriesModel Categories { get; set; }

        [InnerJoin("Suppliers", nameof(SupplierID), nameof(SuppliersModel.SupplierID), "dbo")]
        public SuppliersModel Suppliers { get; set; }
    }
}
