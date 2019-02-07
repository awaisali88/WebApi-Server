using System.Net.Http;
using Common.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI_BAL;
using WebAPI_DataAccess.Context;
using WebAPI_Model.Test;
using WebAPI_ViewModel.DTO;

namespace WebAPI_Server.Controllers
{
    /// <inheritdoc />
    [Route("api/test")]
    [ApiController]
    public class TestRepoController : BaseController
    {
        private IHttpContextAccessor _httpContextAccessor;
        private static readonly HttpClient Client = new HttpClient();

        private readonly ICommonBusinessLogic<IApplicationDbContext, TestRepo, TestRepoViewModel> _cBal;
        private readonly ILogger<TestRepoController> _logger;

        /// <inheritdoc />
        public TestRepoController(IHttpContextAccessor httpContextAccessor, ICommonBusinessLogic<IApplicationDbContext, TestRepo, TestRepoViewModel> cBal,
            ILogger<TestRepoController> logger)
        {
            _cBal = cBal;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [AllowAnonymous]
        public IActionResult AddData([FromBody] TestRepoViewModel data)
        {
            (bool, TestRepoViewModel) result = _cBal.Insert(User, data);
            return Ok(result.Item2, InfoMessages.CommonInfoMessage);
        }

        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        [AllowAnonymous]
        public IActionResult CountData()
        {
            int result = _cBal.Count(x => x.FirstName == "asdfsdf", x => x.FirstName);
            return Ok(result, InfoMessages.CommonInfoMessage);
        }

    }
}