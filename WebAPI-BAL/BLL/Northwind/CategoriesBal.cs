using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_DataAccess.NorthwindContext;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.BLL
{
    public class CategoriesBal : CommonBusinessLogic<INorthwindDbContext, CategoriesModel, CategoriesViewModel>, ICategoriesBal
    {
        private readonly ILogger<CategoriesBal> _logger;

        public CategoriesBal(INorthwindDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor, ILogger<CategoriesBal> logger,
            ILogger<CommonBusinessLogic<INorthwindDbContext, CategoriesModel, CategoriesViewModel>> baseLogger)
            : base(db, mapper, env, httpContextAccessor, baseLogger)
        {
            _logger = logger;
        }
    }
}
