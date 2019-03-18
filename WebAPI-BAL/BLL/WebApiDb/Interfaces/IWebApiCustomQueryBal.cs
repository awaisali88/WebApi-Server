using System;
using System.Collections.Generic;
using System.Text;
using WebAPI_DataAccess.NorthwindContext;
using WebAPI_DataAccess.WebApiContext;

namespace WebAPI_BAL.BLL
{
    public interface IWebApiCustomQueryBal : ICommonStoreProcBusinessLogic<IWebApiDbContext>
    {
    }
}
