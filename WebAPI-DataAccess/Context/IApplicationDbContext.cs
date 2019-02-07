using Dapper.Repositories;
using Dapper.Repositories.DbContext;
using WebAPI_Model.Test;

namespace WebAPI_DataAccess.Context
{
    public interface IApplicationDbContext : IDapperDbContext
    {
        IDapperSProcRepository StoreProcedureRepo { get; }

        IDapperRepository<TestRepo> TestRepo { get; }

        //IDapperRepository<Address> Address { get; }
        //
        //IDapperRepository<User> Users { get; }
        //
        //IDapperRepository<Car> Cars { get; }
        //
        //IDapperRepository<City> Cities { get; }
        //
        //IDapperRepository<Report> Reports { get; }
        //
        //IDapperRepository<Phone> Phones { get; }
    }
}
