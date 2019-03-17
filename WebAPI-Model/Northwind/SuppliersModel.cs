using Dapper.Repositories.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Repositories;

namespace WebAPI_Model
{
    [Table("Suppliers", Schema = "dbo")]
    public class SuppliersModel : DefaultColumns  
    {
        [Identity, Key]  
        public int SupplierID { get; set; }  
  
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
  
        public string HomePage { get; set; }  
  
        public new bool Status { get; set; }  
  
        public bool Trashed { get; set; }  
  
        public DateTime RowVersion { get; set; }  
  
        public DateTime CreatedDate { get; set; }  
  
        public DateTime? ModifiedDate { get; set; }  
  
        public string CreatedBy { get; set; }  
  
        public string ModifiedBy { get; set; }  
  
        public int RecordStatus { get; set; }  
  
    }
}
