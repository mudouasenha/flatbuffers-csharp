using Microsoft.Extensions.DependencyInjection;
using Serialization.Services;

namespace Serialization.CrossCutting
{
    public static class CrossCuttingServiceRegister
    {
        public static IServiceCollection AddCrossCutting(this IServiceCollection services) =>
            services.AddServices();
    }
}