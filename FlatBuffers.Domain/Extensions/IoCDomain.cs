using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services;
using FlatBuffers.Domain.Services.Abstractions;
using FlatBuffers.Receiver.VideoModel;
using Microsoft.Extensions.DependencyInjection;

namespace FlatBuffers.Domain.Extensions
{
    public static class IoCDomain
    {
        public static IServiceCollection AddServices(this IServiceCollection services) =>
            services.AddScoped<ISerializationService<Video, VideoEntity>, SerializationService>()
            .AddScoped<IVideoService, VideoService>();
    }
}