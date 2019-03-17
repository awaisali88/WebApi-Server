using System;

namespace WebAPI_ViewModel.DTO
{
    public class CategoriesViewModel : DefaultViewModel  
    {  
        public int CategoryID { get; set; }  
  
        public string CategoryName { get; set; }  
  
        public string Description { get; set; }  
  
        public byte[] Picture { get; set; }  
  
    }
}
