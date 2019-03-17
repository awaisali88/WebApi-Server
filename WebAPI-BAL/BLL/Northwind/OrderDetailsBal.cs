using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_DataAccess.NorthwindContext;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.BLL
{
    public class OrderDetailsBal : CommonBusinessLogic<INorthwindDbContext, OrderDetailsModel, OrderDetailsViewModel>, IOrderDetailsBal
    {
        private readonly ILogger<OrderDetailsBal> _logger;

        public OrderDetailsBal(INorthwindDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor, ILogger<OrderDetailsBal> logger,
            ILogger<CommonBusinessLogic<INorthwindDbContext, OrderDetailsModel, OrderDetailsViewModel>> baseLogger)
            : base(db, mapper, env, httpContextAccessor, baseLogger)
        {
            _logger = logger;
        }
    }
}
