using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using Common;
using Common.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI_BAL;
using WebAPI_BAL.BLL;
using WebAPI_DataAccess.WebApiContext;
using WebAPI_Model;
using WebAPI_ViewModel.DTO;

namespace WebAPI_Server.Controllers.v2
{
    /// <inheritdoc />
    [ApiVersion(ApiVersionNumber.V2)]
    [Route("api/test")]
    [ApiController]
    public class TestRepoController : BaseController
    {
        private IHttpContextAccessor _httpContextAccessor;
        private static readonly HttpClient Client = new HttpClient();

        private readonly ITestRepoBal _cBal;
        private readonly ICommonStoreProcBusinessLogic<IWebApiDbContext> _cBalProc;
        private readonly ILogger<TestRepoController> _logger;

        /// <inheritdoc />
        public TestRepoController(IHttpContextAccessor httpContextAccessor,
            ITestRepoBal cBal,
            ICommonStoreProcBusinessLogic<IWebApiDbContext> cBalProc,
            ILogger<TestRepoController> logger)
        {
            _cBal = cBal;
            _cBalProc = cBalProc;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("adddata")]
        public IActionResult AddData([FromBody] TestRepoViewModel data)
        {
            (bool, TestRepoViewModel) result = _cBal.Insert(User, data);
            return Ok(result.Item2, InfoMessages.CommonInfoMessage);
        }

        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("updatedata")]
        public IActionResult UpdateData([FromBody] TestRepoViewModel data)
        {
            (bool, TestRepoViewModel) result = _cBal.Update(User, data);
            if (result.Item1)
                return Ok(result.Item2, InfoMessages.CommonInfoMessage);

            //return StatusCodeResult(StatusCodes.Status400BadRequest, result.Item2, ErrorMessages.RecordNotFoundUpdate);

            return BadRequest(ErrorMessages.RecordNotFoundUpdate, result.Item2);
        }

        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("deletedata")]
        public IActionResult DeleteData([FromBody] TestRepoViewModel data)
        {
            //object dd = _cBal.HandleTransaction((IDbTransaction x) => { return null as object; });

            bool result = _cBal.HandleTransaction((IDbTransaction trans) =>
            {
                TestRepoViewModel trData = _cBal.FindById(data.Id, transaction: trans);
                return _cBal.Delete(User, trData, transaction: trans);
            });

            //bool result = _cBal.Delete(User, x => x.Id == data.Id && x.RowVersion == data.RowVersion);
            if (result)
                return Ok(true, InfoMessages.CommonInfoMessage);

            return BadRequest("Error in deleting model");
        }

        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        [AllowAnonymous]
        public IActionResult CountData()
        {
            int result = _cBal.Count(true);
            return Ok(result, InfoMessages.CommonInfoMessage);
        }

        /// <summary>
        /// Select All
        /// </summary>
        /// <returns></returns>
        [HttpGet("selectall")]
        [AllowAnonymous]
        public IActionResult SelectAll()
        {
            var result = _cBal.FindAll(includeLogicalDeleted: true);
            return Ok(result, InfoMessages.CommonInfoMessage);
        }
    }
}