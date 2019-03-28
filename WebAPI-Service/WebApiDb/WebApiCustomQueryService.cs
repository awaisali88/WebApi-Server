using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_BAL.BLL;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_Service.WebApiDb
{
    public class WebApiCustomQueryService : IWebApiCustomQueryService
    {
        private readonly ILogger<WebApiCustomQueryService> _logger;
		private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

		private readonly IWebApiCustomQueryBal _webApiCustomQueryBal;

        public WebApiCustomQueryService(IHostingEnvironment env, IHttpContextAccessor httpContextAccessor, ILogger<WebApiCustomQueryService> logger
			, IWebApiCustomQueryBal webApiCustomQueryBal)
        {
			_env = env;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

			_webApiCustomQueryBal = webApiCustomQueryBal;
        }
    }
}
