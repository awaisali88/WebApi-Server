using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_DataAccess.NorthwindContext;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.BLL
{
    public class ShippersBal : CommonBusinessLogic<INorthwindDbContext, ShippersModel, ShippersViewModel>, IShippersBal
    {
        private readonly ILogger<ShippersBal> _logger;

        public ShippersBal(INorthwindDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor, ILogger<ShippersBal> logger,
            ILogger<CommonBusinessLogic<INorthwindDbContext, ShippersModel, ShippersViewModel>> baseLogger)
            : base(db, mapper, env, httpContextAccessor, baseLogger)
        {
            _logger = logger;
        }
    }
}
