using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI_BAL.BLL;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_Service.WebApiDb
{
    public class TestRepoService : ITestRepoService
    {
        private readonly ILogger<TestRepoService> _logger;
		private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

		private readonly ITestRepoBal _testRepoBal;

        public TestRepoService(IHostingEnvironment env, IHttpContextAccessor httpContextAccessor, ILogger<TestRepoService> logger
			, ITestRepoBal testRepoBal)
        {
			_env = env;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

			_testRepoBal = testRepoBal;
        }
    }
}
