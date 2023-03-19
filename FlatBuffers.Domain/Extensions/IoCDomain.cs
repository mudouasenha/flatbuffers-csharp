using FlatBuffers.Domain.Services;
using FlatBuffers.Domain.Services.Abstractions;
using FlatBuffers.Domain.Services.Converters.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace FlatBuffers.Domain.Extensions
{
    public static class IoCDomain
    {
        public static IServiceCollection AddServices(this IServiceCollection services) =>
            services.AddTransient<IFlatBuffersVideoConverter, VideoFlatBuffersConverter>()
            .AddScoped<IVideoService, VideoService>();
    }
}