using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_DataAccess.NorthwindContext;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.BLL
{
    public class EmployeesBal : CommonBusinessLogic<INorthwindDbContext, EmployeesModel, EmployeesViewModel>, IEmployeesBal
    {
        private readonly ILogger<EmployeesBal> _logger;

        public EmployeesBal(INorthwindDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor, ILogger<EmployeesBal> logger,
            ILogger<CommonBusinessLogic<INorthwindDbContext, EmployeesModel, EmployeesViewModel>> baseLogger)
            : base(db, mapper, env, httpContextAccessor, baseLogger)
        {
            _logger = logger;
        }
    }
}
