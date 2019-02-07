using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebAPI_BAL.AuthLogic;
using WebAPI_BAL.NotificationManager;
using WebAPI_DataAccess.Context;
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

            //DbContext
            services.TryAddTransient<IApplicationDbContext, ApplicationDbContext>();

            //Service
            services.TryAddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}
