using Microsoft.Extensions.DependencyInjection;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;

namespace Serialization.Services
{
    public static class IoCServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services) =>
            services.AddFlatBuffers()
            .AddScoped<IVideoService, VideoService>()
            .AddScoped<SenderService>()
            .AddHttpClients();

        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<ReceiverClient>(httpClient => httpClient.BaseAddress = new Uri("https://localhost:5021"));

            return services;
        }
    }
}