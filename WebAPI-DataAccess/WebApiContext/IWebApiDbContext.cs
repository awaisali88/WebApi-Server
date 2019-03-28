using Dapper.Repositories;
using Dapper.Repositories.DbContext;
using WebAPI_Model.Test;

namespace WebAPI_DataAccess.WebApiContext
{
    public interface IWebApiDbContext : IDapperDbContext
    {
        #region Auto Generated Code. Don't Delete or Modify this section
        IDapperRepository<TestRepo> TestRepo { get; }
        //[AUTO_GENERATED_REPO_WebApiDb]
        #endregion
    }
}
