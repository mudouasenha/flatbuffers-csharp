using Microsoft.Extensions.DependencyInjection;
using Serialization.Serializers.MessagePack;

namespace Serialization.Serializers.FlatBuffers
{
    public static class IoCMessagePack
    {
        public static IServiceCollection AddMessagePack(this IServiceCollection services) =>
            services.AddScoped<MessagePackCSharpSerializer>();
    }
}