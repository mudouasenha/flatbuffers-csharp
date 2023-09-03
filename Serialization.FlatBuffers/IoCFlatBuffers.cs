using Microsoft.Extensions.DependencyInjection;

namespace Serialization.Serializers.FlatBuffers
{
    public static class IoCFlatBuffers
    {
        public static IServiceCollection AddFlatBuffers(this IServiceCollection services) =>
            services.AddScoped<VideoFlatBuffersSerializer>();
    }

}