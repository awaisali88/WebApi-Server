using Dapper.Repositories;
using WebAPI_Model.Test;

namespace WebAPI_DataAccess.WebApiContext
{
    public partial class WebApiDbContext
    {
        #region Auto Generated Code. Don't Delete or Modify this section
        public IDapperRepository<TestRepo> TestRepo => _testRepo ?? (_testRepo = GetRepository<TestRepo>());
        //[AUTO_GENERATED_REPO_WebApiDb]
        #endregion
    }
}
