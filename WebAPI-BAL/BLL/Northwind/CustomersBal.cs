using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_DataAccess.NorthwindContext;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.BLL
{
    public class CustomersBal : CommonBusinessLogic<INorthwindDbContext, CustomersModel, CustomersViewModel>, ICustomersBal
    {
        private readonly ILogger<CustomersBal> _logger;

        public CustomersBal(INorthwindDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor, ILogger<CustomersBal> logger,
            ILogger<CommonBusinessLogic<INorthwindDbContext, CustomersModel, CustomersViewModel>> baseLogger)
            : base(db, mapper, env, httpContextAccessor, baseLogger)
        {
            _logger = logger;
        }
    }
}
