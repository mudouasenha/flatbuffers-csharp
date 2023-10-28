using Microsoft.Extensions.DependencyInjection;
using Serialization.Serializers.FlatBuffers.SerializationHelpers;

namespace Serialization.Serializers.FlatBuffers
{
    public static class IoCFlatBuffers
    {
        public static IServiceCollection AddFlatBuffers(this IServiceCollection services) =>
            services.AddScoped<VideoFlatBuffersSerializer>();
    }
}