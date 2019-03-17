using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_DataAccess.NorthwindContext;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.BLL
{
    public class ProductsBal : CommonBusinessLogic<INorthwindDbContext, ProductsModel, ProductsViewModel>, IProductsBal
    {
        private readonly ILogger<ProductsBal> _logger;

        public ProductsBal(INorthwindDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor, ILogger<ProductsBal> logger,
            ILogger<CommonBusinessLogic<INorthwindDbContext, ProductsModel, ProductsViewModel>> baseLogger)
            : base(db, mapper, env, httpContextAccessor, baseLogger)
        {
            _logger = logger;
        }
    }
}
