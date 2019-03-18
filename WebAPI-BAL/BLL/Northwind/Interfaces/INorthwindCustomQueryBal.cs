using System;
using System.Collections.Generic;
using System.Text;
using WebAPI_DataAccess.NorthwindContext;

namespace WebAPI_BAL.BLL
{
    public interface INorthwindCustomQueryBal : ICommonStoreProcBusinessLogic<INorthwindDbContext>
    {
    }
}
