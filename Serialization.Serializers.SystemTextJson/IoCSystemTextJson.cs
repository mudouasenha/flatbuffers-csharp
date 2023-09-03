using Microsoft.Extensions.DependencyInjection;

namespace Serialization.Serializers.SystemTextJson
{
    public static class IoCSystemTextJson
    {
        public static IServiceCollection AddSystemTextJsonSerializer(this IServiceCollection services) =>
            services.AddScoped<SytemTextJsonSerializer>();
    }
}
