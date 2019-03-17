using AutoMapper;
using Dapper.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_DataAccess.NorthwindContext;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.BLL
{
    public class OrdersBal : CommonBusinessLogic<INorthwindDbContext, OrdersModel, OrdersViewModel>, IOrdersBal
    {
        private readonly ILogger<OrdersBal> _logger;

        public OrdersBal(INorthwindDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor, ILogger<OrdersBal> logger,
            ILogger<CommonBusinessLogic<INorthwindDbContext, OrdersModel, OrdersViewModel>> baseLogger)
            : base(db, mapper, env, httpContextAccessor, baseLogger)
        {
            _logger = logger;
        }
    }
}
