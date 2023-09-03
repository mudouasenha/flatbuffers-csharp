using Microsoft.Extensions.DependencyInjection;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.FlatBuffers
{
    public static class IoCFlatBuffers
    {
        public static IServiceCollection AddFlatBuffers(this IServiceCollection services) =>
            services.AddScoped<IFlatBuffersVideoSerializer, VideoFlatBuffersSerializer>();
    }

}