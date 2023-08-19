using FlatBuffers.Domain.Services;
using FlatBuffers.Domain.Services.Abstractions;
using FlatBuffers.Domain.Services.Flatbuffers;
using FlatBuffers.Domain.Services.Flatbuffers.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Serialization.Domain.Extensions
{
    public static class IoCDomain
    {
        public static IServiceCollection AddServices(this IServiceCollection services) =>
            services.AddScoped<IFlatBuffersVideoConverter, VideoFlatBuffersConverter>()
            .AddScoped<IVideoService, VideoService>();
    }
}