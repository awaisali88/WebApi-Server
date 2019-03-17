using WebAPI_DataAccess.NorthwindContext;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.BLL
{
    public interface IOrderDetailsBal : ICommonBusinessLogic<INorthwindDbContext, OrderDetailsModel, OrderDetailsViewModel>
    {
    }
}
