using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using Common;
using Common.Messages;
using Dapper;
using MicroElements.Swashbuckle.FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using WebAPI_BAL;
using WebAPI_BAL.BLL;
using WebAPI_DataAccess.WebApiContext;
using WebAPI_Model;
using WebAPI_Server.AppStart;
using WebAPI_ViewModel.DTO;

namespace WebAPI_Server.Controllers.v1
{
    /// <inheritdoc />
    [ApiVersion(ApiVersionNumber.V1)]
    [Route("api/test")]
    [ApiController]
    public class TestRepoController : BaseController
    {
        private IHttpContextAccessor _httpContextAccessor;
        private static readonly HttpClient Client = new HttpClient();

        private readonly ITestRepoBal _testRepoBal;
        private readonly ICommonStoreProcBusinessLogic<IWebApiDbContext> _cBalProc;
        private readonly ILogger<TestRepoController> _logger;
        private readonly IOrdersBal _ordersBal;
        private readonly IOrderDetailsBal _orderDetailsBal;
        private readonly ICustomersBal _customersBal;

        /// <inheritdoc />
        public TestRepoController(IHttpContextAccessor httpContextAccessor,
            ITestRepoBal testRepoBal,
            ICommonStoreProcBusinessLogic<IWebApiDbContext> cBalProc,
            ILogger<TestRepoController> logger, IOrdersBal ordersBal, IOrderDetailsBal orderDetailsBal, ICustomersBal customersBal)
        {
            _testRepoBal = testRepoBal;
            _cBalProc = cBalProc;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            _ordersBal = ordersBal;
            _orderDetailsBal = orderDetailsBal;
            _customersBal = customersBal;

            //Get Service Object from DI
            //private readonly IServiceProvider _serviceProvider; //get from Constructor
            //var serviceName = _serviceProvider.GetService<IService>();
        }

        /// <summary>
        /// Test API for repository
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("adddata")]
        public IActionResult AddData([FromBody] TestRepoViewModel data)
        {
            (bool, TestRepoViewModel) result = _testRepoBal.Insert(User, data);
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
            (bool, TestRepoViewModel) result = _testRepoBal.Update(User, x => new {x.FirstName, x.LastName }, data);
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
        [HttpPut("updatetestdata")]
        [AllowAnonymous]
        [AllowNoAccessToken]
        public IActionResult UpdateTestData()
        {
            //var ids = new List<int> { 33, 34 };
            List<int> testId = new List<int> { 38, 39, 40 };

            //(bool, TestRepoViewModel) result = _testRepoBal.Update(User, x=> x.FirstName.StartsWith("asd"), 
            //    x => new { x.FirstName, x.LastName },
            //    new TestRepoViewModel(){FirstName = "Update With In Query",LastName = "Last Name IN Q"});
            //if (result.Item1)
            //    return Ok(result.Item2, InfoMessages.CommonInfoMessage);

            var allData = _testRepoBal.FindAll();

            //Working
            //var rowVersion = allData.FirstOrDefault(x => x.Id == 33).RowVersion;
            //var data = _testRepoBal.Update(User, x => x.Id == 33, x => x.FirstName,
            //    new TestRepoViewModel() {FirstName = "Update With In Query 123465", RowVersion = rowVersion });

            //Not Working
            //var rowVersion = allData.FirstOrDefault(x => x.FirstName == "Update With In Query").RowVersion;
            //var data = _testRepoBal.Update(User, x => x.FirstName == "Update With In Query", x => x.FirstName,
            //    new TestRepoViewModel() { FirstName = "Update With In Query 123", RowVersion = rowVersion });

            //Working
            //var data = _testRepoBal.Update(User, x => testId.Contains(x.Id), x => x.FirstName,
            //    new TestRepoViewModel() { FirstName = "Update With In Query 456" });
            var data1 = _testRepoBal.FindAll(x => testId.Contains(x.Id));
            var data2 = _testRepoBal.FindAll(x => testId.NotContains(x.Id));

            return Ok(data2, InfoMessages.CommonInfoMessage);

            //return StatusCodeResult(StatusCodes.Status400BadRequest, result.Item2, ErrorMessages.RecordNotFoundUpdate);

            //return BadRequest(ErrorMessages.RecordNotFoundUpdate, result.Item2);
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

            bool result = _testRepoBal.HandleTransaction((IDbTransaction trans) =>
            {
                TestRepoViewModel trData = _testRepoBal.FindById(data.Id, transaction: trans);
                return _testRepoBal.Delete(User, trData, transaction: trans);
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
            int result = _testRepoBal.Count();
            return Ok(result, InfoMessages.CommonInfoMessage);
        }

        /// <summary>
        /// Select All
        /// </summary>
        /// <returns></returns>
        [HttpGet("selectall")]
        [AllowAnonymous]
        [AllowNoAccessToken]
        public IActionResult SelectAll()
        {
            var result = _testRepoBal.FindAll();
            return Ok(result, InfoMessages.CommonInfoMessage);
        }

        /// <summary>
        /// Select all Orders
        /// </summary>
        /// <returns></returns>
        [HttpGet("selectorders")]
        [AllowAnonymous]
        [AllowNoAccessToken]
        public IActionResult SelectOrders()
        {
            //Working
            //var result = _ordersBal
            //    .FindAll<CustomersModel, EmployeesModel, ShippersModel>(
            //        x => x.ShipVia == 2 && x.Customers.Country == "Germany", x => x.Customers, x => x.Employees,
            //        x => x.Shippers).FirstOrDefault();

            var result = _ordersBal
                .FindAll<CustomersModel, EmployeesModel, ShippersModel>(
                    x => x.ShipVia == 2 && x.Customers.Country == x.Customers.Country, x => x.Customers, x => x.Employees,
                    x => x.Shippers).Data.FirstOrDefault();

            //Not Working (Use Db Query from Dapper) (Where clause on Level 2 join)
            //var result = _orderDetailsBal.FindAll<OrdersModel, CustomersModel>(x => x.Orders.Customers.Country == "Germany", 
            //    x => x.Orders, x => x.Orders.Customers);

            //Working (Composite primary key columns issue, Worked with split on different key column from linked Foreign key)
            //var result = _ordersBal.FindAll<CustomersModel, OrderDetailsModel>(x => x.Customers.Country == "Germany",
            //    x => x.Customers, x => x.OrderDetails);


            //Working (List of child objects from join)
            //var result1 = _customersBal.FindAll<OrdersModel>(x => x.CustomerID == "VINET",
            //    x => x.OrdersModel);


            //Working (Composite primary key columns issue, Worked with split on different key column from linked Foreign key)
            //var result = _ordersBal.FindAll<OrderDetailsModel>(x => x.OrderID == 10248, x => x.OrderDetails);

            //Working (Composite primary key columns issue, Worked with split on different key column from linked Foreign key)
            //string sqlQuery =
            //    "SELECT [dbo].[Orders].[OrderID], [dbo].[Orders].[CustomerID], [dbo].[Orders].[EmployeeID], [dbo].[Orders].[OrderDate], [dbo].[Orders].[RequiredDate], [dbo].[Orders].[ShippedDate], [dbo].[Orders].[ShipVia], [dbo].[Orders].[Freight], [dbo].[Orders].[ShipName], [dbo].[Orders].[ShipAddress], [dbo].[Orders].[ShipCity], [dbo].[Orders].[ShipRegion], [dbo].[Orders].[ShipPostalCode], [dbo].[Orders].[ShipCountry], [dbo].[Orders].[Status] AS Status, [dbo].[Orders].[Trashed] AS Trashed, [dbo].[Orders].[RowVersion], [dbo].[Orders].[CreatedDate] AS CreatedDate, [dbo].[Orders].[ModifiedDate] AS ModifiedDate, [dbo].[Orders].[CreatedBy] AS CreatedBy, [dbo].[Orders].[ModifiedBy] AS ModifiedBy, [dbo].[Orders].[RecordStatus] AS RecordStatusCode, [OrderDetails_OrderID].[OrderID], [OrderDetails_OrderID].[ProductID], [OrderDetails_OrderID].[UnitPrice], [OrderDetails_OrderID].[Quantity], [OrderDetails_OrderID].[Discount], [OrderDetails_OrderID].[Status] AS Status, [OrderDetails_OrderID].[Trashed] AS Trashed, [OrderDetails_OrderID].[RowVersion], [OrderDetails_OrderID].[CreatedDate] AS CreatedDate, [OrderDetails_OrderID].[ModifiedDate] AS ModifiedDate, [OrderDetails_OrderID].[CreatedBy] AS CreatedBy, [OrderDetails_OrderID].[ModifiedBy] AS ModifiedBy, [OrderDetails_OrderID].[RecordStatus] AS RecordStatusCode FROM [dbo].[Orders] LEFT JOIN [dbo].[OrderDetails] AS [OrderDetails_OrderID] ON [Orders].[OrderID] = [OrderDetails_OrderID].[OrderID] WHERE [dbo].[Orders].[OrderID] = @OrderId_where AND [dbo].[Orders].[Trashed] != 1";
            //
            //List<OrdersModel> orderData;
            //using (var conn = _ordersBal.Conn)
            //{
            //    var orderDictionary = new Dictionary<int, OrdersModel>();
            //    orderData = conn.Query<OrdersModel, OrderDetailsModel, OrdersModel>(
            //            sqlQuery,
            //            (order, orderDetail) =>
            //            {
            //                OrdersModel orderEntry;
            //
            //                if (!orderDictionary.TryGetValue(order.OrderID, out orderEntry))
            //                {
            //                    orderEntry = order;
            //                    orderEntry.OrderDetails = new List<OrderDetailsModel>();
            //                    orderDictionary.Add(orderEntry.OrderID, orderEntry);
            //                }
            //
            //                orderEntry.OrderDetails.Add(orderDetail);
            //                return orderEntry;
            //            },
            //            splitOn: "OrderID", param: new{ ORderId_where = 10248 })
            //        .Distinct()
            //        .ToList();
            //}



            bool r = false;
            return Ok(r, InfoMessages.CommonInfoMessage);
        }

    }
}