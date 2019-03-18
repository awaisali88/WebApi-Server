using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_DataAccess.WebApiContext;

namespace WebAPI_BAL.BLL
{
    public class WebApiCustomQueryBal : CommonStoreProcBusinessLogic<IWebApiDbContext>, IWebApiCustomQueryBal
    {
        private readonly ILogger<WebApiCustomQueryBal> _logger;

        public WebApiCustomQueryBal(IWebApiDbContext db, IMapper mapper, IHostingEnvironment env,
            IHttpContextAccessor httpContextAccessor, ILogger<WebApiCustomQueryBal> logger,
            ILogger<CommonStoreProcBusinessLogic<IWebApiDbContext>> baseLogger
        )
            : base(db, mapper, env, httpContextAccessor, baseLogger)
        {
            _logger = logger;
        }
    }
}
