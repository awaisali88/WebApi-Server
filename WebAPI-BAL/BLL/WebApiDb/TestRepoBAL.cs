using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_DataAccess.WebApiContext;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_BAL.BLL
{
    public class TestRepoBal : CommonBusinessLogic<IWebApiDbContext, TestRepoModel, TestRepoViewModel>, ITestRepoBal
    {
        private readonly ILogger<TestRepoBal> _logger;

        public TestRepoBal(IWebApiDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor, ILogger<TestRepoBal> logger,
            ILogger<CommonBusinessLogic<IWebApiDbContext, TestRepoModel, TestRepoViewModel>> baseLogger)
            : base(db, mapper, env, httpContextAccessor, baseLogger)
        {
            _logger = logger;
        }
    }
}
