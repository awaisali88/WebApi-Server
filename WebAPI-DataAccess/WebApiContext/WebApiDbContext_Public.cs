using Dapper.Repositories;
using WebAPI_Model.Test;

namespace WebAPI_DataAccess.WebApiContext
{
    public partial class WebApiDbContext
    {
        #region Public Repository Properties
        public IDapperSProcRepository StoreProcedureRepo => _spRepo ?? (_spRepo = new DapperSProcRepository(Connection));
        public IDapperRepository<TestRepo> TestRepo => _testRepo ?? (_testRepo = new DapperRepository<TestRepo>(Connection, _mapper, _config));
        #endregion

        #region Auto Generated Code. Don't Delete or Modify this section
        //[AUTO_GENERATED_REPO_WebApiDb]
        #endregion
    }
}
