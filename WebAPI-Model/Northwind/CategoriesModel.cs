using Dapper.Repositories.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories;

namespace WebAPI_Model
{
    [Table("Categories", Schema = "dbo")]
    public class CategoriesModel : DefaultColumns
    {
        [Identity, Key]  
        public int CategoryID { get; set; }  
  
        public string CategoryName { get; set; }  
  
        public string Description { get; set; }  
  
        public byte[] Picture { get; set; }  
  
    }
}
