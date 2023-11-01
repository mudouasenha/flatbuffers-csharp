using Serialization.Domain;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.ApacheAvro;
using Serialization.Serializers.CapnProto;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.MessagePack;
using Serialization.Serializers.SystemTextJson;

namespace Serialization.Receiver.Services
{
    public static class SerializerFactory
    {
        private static readonly Dictionary<string, ISerializer> Serializers = new Dictionary<string, ISerializer>
    {
        { "FlatBuffers", new FlatBuffersSerializer() },
        { "Avro", new ApacheAvroSerializer() },
        { "Thrift", new ApacheThriftSerializer() },
        { "MessagePack-CSharp", new MessagePackCSharpSerializer() },
        { "CapnProto", new CapnProtoSerializer() },
        { "Newtonsoft.Json", new NewtonsoftJsonSerializer() },
        { "Protobuf", new ProtobufSerializer() },
        // Adicione outros tipos e implementações aqui
    };

        public static ISerializer GetSerializer(string type)
        {
            if (Serializers.TryGetValue(type, out var serializer))
            {
                return serializer;
            }

            throw new ArgumentException("Unsupported target type");
        }
    }
}