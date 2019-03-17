using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_DataAccess.NorthwindContext;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.BLL
{
    public class SuppliersBal : CommonBusinessLogic<INorthwindDbContext, SuppliersModel, SuppliersViewModel>, ISuppliersBal
    {
        private readonly ILogger<SuppliersBal> _logger;

        public SuppliersBal(INorthwindDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor, ILogger<SuppliersBal> logger,
            ILogger<CommonBusinessLogic<INorthwindDbContext, SuppliersModel, SuppliersViewModel>> baseLogger)
            : base(db, mapper, env, httpContextAccessor, baseLogger)
        {
            _logger = logger;
        }
    }
}
