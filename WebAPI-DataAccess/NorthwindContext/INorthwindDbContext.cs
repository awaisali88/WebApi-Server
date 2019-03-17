using Dapper.Repositories;
using Dapper.Repositories.DbContext;
using WebAPI_Model;

namespace WebAPI_DataAccess.NorthwindContext
{
    public interface INorthwindDbContext : IDapperDbContext
    {
        IDapperSProcRepository StoreProcedureRepo { get; }

        #region Auto Generated Code. Don't Delete or Modify this section
        IDapperRepository<CategoriesModel> CategoriesRepo { get; }
        IDapperRepository<EmployeesModel> EmployeesRepo { get; }
        IDapperRepository<ShippersModel> ShippersRepo { get; }
        IDapperRepository<CustomersModel> CustomersRepo { get; }
        IDapperRepository<OrdersModel> OrdersRepo { get; }
        IDapperRepository<OrderDetailsModel> OrderDetailsRepo { get; }
        IDapperRepository<ProductsModel> ProductsRepo { get; }
        IDapperRepository<SuppliersModel> SuppliersRepo { get; }
        //[AUTO_GENERATED_REPO_Northwind]
        #endregion
    }
}
