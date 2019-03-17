using Dapper.Repositories;
using WebAPI_Model.Test;

namespace WebAPI_DataAccess.WebApiContext
{
    public partial class WebApiDbContext
    {
        #region Private Repository Properties
        private IDapperSProcRepository _spRepo;
        private IDapperRepository<TestRepo> _testRepo;
        #endregion

        #region Auto Generated Code. Don't Delete or Modify this section
        //[AUTO_GENERATED_REPO_WebApiDb]
        #endregion
    }
}
