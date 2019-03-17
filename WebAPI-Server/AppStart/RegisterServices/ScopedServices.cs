using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebAPI_BAL.ApplicationBAL;
using WebAPI_BAL.AuthLogic;
using WebAPI_BAL.BLL;
using WebAPI_BAL.NotificationManager;
using WebAPI_DataAccess.NorthwindContext;
using WebAPI_DataAccess.WebApiContext;
using WebAPI_Service.Service;

namespace WebAPI_Server.AppStart
{
    internal static partial class ServiceInjection
    {
        internal static void ScopedServices(IServiceCollection services)
        {
            #region NotificationManager
            services.AddScoped<IEmailSender, AuthMessageSender>();
            services.AddScoped<ISmsSender, AuthMessageSender>();
            #endregion

            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.TryAddScoped<IAuthentication, Authentication>();
            services.TryAddScoped<IAuthorization, Authorization>();

            #region Database
            services.TryAddTransient<IWebApiDbContext, WebApiDbContext>();
            services.TryAddTransient<INorthwindDbContext, NorthwindDbContext>();
            #endregion

            #region Api Services [Auto Generated Code. Don't Delete or Modify this section]
            services.TryAddScoped<IAuthenticationService, AuthenticationService>();
			//[AUTO_GENERATED_SCOPED_ApiServices]
            #endregion

            #region Auto Generated Code. Don't Delete or Modify this section
            #region WebApiDb Bal AG
            services.TryAddScoped<ITestRepoBal, TestRepoBal>();
            //[AUTO_GENERATED_SCOPED_SERVICES_WebApiDb]
            #endregion
            #region Northwind Bal AG
            services.TryAddScoped<ICategoriesBal, CategoriesBal>();
			services.TryAddScoped<IEmployeesBal, EmployeesBal>();
			services.TryAddScoped<IShippersBal, ShippersBal>();
			services.TryAddScoped<ICustomersBal, CustomersBal>();
			services.TryAddScoped<IOrdersBal, OrdersBal>();
			services.TryAddScoped<IOrderDetailsBal, OrderDetailsBal>();
			services.TryAddScoped<IProductsBal, ProductsBal>();
			services.TryAddScoped<ISuppliersBal, SuppliersBal>();
			//[AUTO_GENERATED_SCOPED_SERVICES_Northwind]
            #endregion
            #endregion

        }
    }
}
