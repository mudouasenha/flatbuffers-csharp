using Microsoft.Extensions.DependencyInjection;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.SystemTextJson;

namespace Serialization.Services
{
    public static class IoCServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services) =>
            services.AddFlatBuffers()
            .AddSystemTextJsonSerializer()
            .AddScoped<IVideoService, VideoService>()
            .AddScoped<SenderService>();
    }
}