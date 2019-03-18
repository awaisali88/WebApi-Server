using System;
using System.Collections.Generic;
using System.Text;
using WebAPI_DataAccess.WebApiContext;
using WebAPI_Model.Test;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.BLL
{
    public interface ITestRepoBal : ICommonBusinessLogic<IWebApiDbContext, TestRepo, TestRepoViewModel>
    {
    }
}
