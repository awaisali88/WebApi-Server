using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_BAL.BLL;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_Service.Northwind
{
    public class NorthwindCustomQueryService : INorthwindCustomQueryService
    {
        private readonly ILogger<NorthwindCustomQueryService> _logger;
		private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

		private readonly INorthwindCustomQueryBal _northwindCustomQueryBal;

        public NorthwindCustomQueryService(IHostingEnvironment env, IHttpContextAccessor httpContextAccessor, ILogger<NorthwindCustomQueryService> logger
			, INorthwindCustomQueryBal northwindCustomQueryBal)
        {
			_env = env;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

			_northwindCustomQueryBal = northwindCustomQueryBal;
        }
    }
}
