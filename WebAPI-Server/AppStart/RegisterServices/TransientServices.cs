using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebAPI_BAL;

namespace WebAPI_Server.AppStart
{
    internal static partial class ServiceInjection
    {
        internal static void TransientServices(IServiceCollection services)
        {
            //BAL
            services.TryAddTransient(typeof(ICommonBusinessLogic<,,>), typeof(CommonBusinessLogic<,,>));
            services.TryAddTransient(typeof(ICommonStoreProcBusinessLogic<>), typeof(CommonStoreProcBusinessLogic<>));
        }
    }
}
