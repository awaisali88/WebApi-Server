using Microsoft.Extensions.DependencyInjection;
using WebAPI_BAL.JwtGenerator;

namespace WebAPI_Server.AppStart
{
    internal static partial class ServiceInjection
    {
        internal static void SingletonServices(IServiceCollection services)
        {
            services.AddSingleton<IJwtFactory, JwtFactory>();
        }
    }
}
