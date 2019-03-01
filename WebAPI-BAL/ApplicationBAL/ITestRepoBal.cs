using System;
using System.Collections.Generic;
using System.Text;
using WebAPI_DataAccess.Context;
using WebAPI_Model.Test;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.ApplicationBAL
{
    public interface ITestRepoBal : ICommonBusinessLogic<IApplicationDbContext, TestRepo, TestRepoViewModel>
    {
    }
}
