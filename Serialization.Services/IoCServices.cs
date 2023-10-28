using Microsoft.Extensions.DependencyInjection;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.SystemTextJson;

namespace Serialization.Services
{
    public static class IoCServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services) =>
            services.AddFlatBuffers()
            .AddSystemTextJsonSerializer()
            .AddMessagePack()
            .AddScoped<WorkloadService>();
    }
}