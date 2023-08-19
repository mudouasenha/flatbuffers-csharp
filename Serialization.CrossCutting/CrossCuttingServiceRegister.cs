using Microsoft.Extensions.DependencyInjection;
using Serialization.Domain.Extensions;

namespace Serialization.CrossCutting
{
    public static class CrossCuttingServiceRegister
    {
        public static IServiceCollection AddCrossCutting(this IServiceCollection services) =>
            services.AddServices();
    }
}