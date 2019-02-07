using Dapper.Repositories;
using WebAPI_Model.Test;

namespace WebAPI_DataAccess.Context
{
    public partial class ApplicationDbContext
    {
        #region Private Repository Properties
        //private IDapperRepository<Address> _address;
        //private IDapperRepository<Car> _cars;
        //private IDapperRepository<User> _users;
        //private IDapperRepository<City> _cities;
        //private IDapperRepository<Report> _reports;
        //private IDapperRepository<Phone> _phones;

        private IDapperSProcRepository _spRepo;
        private IDapperRepository<TestRepo> _testRepo;
        #endregion
    }
}
