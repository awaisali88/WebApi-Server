using Dapper.Repositories;
using Dapper.Repositories.DbContext;
using WebAPI_Model.Test;

namespace WebAPI_DataAccess.WebApiContext
{
    public interface IWebApiDbContext : IDapperDbContext
    {
        IDapperSProcRepository StoreProcedureRepo { get; }
        IDapperRepository<TestRepo> TestRepo { get; }

        #region Auto Generated Code. Don't Delete or Modify this section
        //[AUTO_GENERATED_REPO_WebApiDb]
        #endregion
    }
}
