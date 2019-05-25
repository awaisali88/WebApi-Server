using WebAPI_DataAccess.WebApiContext;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.BLL
{
    public interface ITestRepoBal : ICommonBusinessLogic<IWebApiDbContext, TestRepoModel, TestRepoViewModel>
    {
    }
}
