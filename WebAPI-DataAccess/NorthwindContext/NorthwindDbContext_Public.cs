using Dapper.Repositories;
using WebAPI_Model;

namespace WebAPI_DataAccess.NorthwindContext
{
    public partial class NorthwindDbContext
    {
        #region Public Repository Properties
        public IDapperSProcRepository StoreProcedureRepo => _spRepo ?? (_spRepo = new DapperSProcRepository(Connection));
        #endregion

        #region Auto Generated Code. Don't Delete or Modify this section
        public IDapperRepository<CategoriesModel> CategoriesRepo => _categoriesRepo ?? (_categoriesRepo = new DapperRepository<CategoriesModel>(Connection, _mapper, _config));
        public IDapperRepository<EmployeesModel> EmployeesRepo => _employeesRepo ?? (_employeesRepo = new DapperRepository<EmployeesModel>(Connection, _mapper, _config));
        public IDapperRepository<ShippersModel> ShippersRepo => _shippersRepo ?? (_shippersRepo = new DapperRepository<ShippersModel>(Connection, _mapper, _config));
        public IDapperRepository<CustomersModel> CustomersRepo => _customersRepo ?? (_customersRepo = new DapperRepository<CustomersModel>(Connection, _mapper, _config));
        public IDapperRepository<OrdersModel> OrdersRepo => _ordersRepo ?? (_ordersRepo = new DapperRepository<OrdersModel>(Connection, _mapper, _config));
        public IDapperRepository<OrderDetailsModel> OrderDetailsRepo => _orderDetailsRepo ?? (_orderDetailsRepo = new DapperRepository<OrderDetailsModel>(Connection, _mapper, _config));
        public IDapperRepository<ProductsModel> ProductsRepo => _productsRepo ?? (_productsRepo = new DapperRepository<ProductsModel>(Connection, _mapper, _config));
        public IDapperRepository<SuppliersModel> SuppliersRepo => _suppliersRepo ?? (_suppliersRepo = new DapperRepository<SuppliersModel>(Connection, _mapper, _config));
        //[AUTO_GENERATED_REPO_Northwind]
        #endregion
    }
}
