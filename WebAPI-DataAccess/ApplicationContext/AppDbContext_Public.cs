using Dapper.Repositories;
using WebAPI_Model.Test;

namespace WebAPI_DataAccess.ApplicationContext
{
    public partial class ApplicationDbContext
    {
        #region Public Repository Properties
        //public IDapperRepository<Address> Address => _address ?? (_address = new DapperRepository<Address>(Connection, _config));
        //public IDapperRepository<User> Users => _users ?? (_users = new DapperRepository<User>(Connection, _config));
        //public IDapperRepository<Car> Cars => _cars ?? (_cars = new DapperRepository<Car>(Connection, _config));
        //public IDapperRepository<City> Cities => _cities ?? (_cities = new DapperRepository<City>(Connection, _config));
        //public IDapperRepository<Report> Reports => _reports ?? (_reports = new DapperRepository<Report>(Connection, _config));
        //public IDapperRepository<Phone> Phones => _phones ?? (_phones = new DapperRepository<Phone>(Connection, _config));

        public IDapperSProcRepository StoreProcedureRepo => _spRepo ?? (_spRepo = new DapperSProcRepository(Connection));
        public IDapperRepository<TestRepo> TestRepo => _testRepo ?? (_testRepo = new DapperRepository<TestRepo>(Connection, _config));
        #endregion

    }
}
