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
        private static readonly FlatBuffersSerializer flatBuffers = new FlatBuffersSerializer();
        private static readonly ApacheAvroSerializer avro = new ApacheAvroSerializer();
        private static readonly ApacheThriftSerializer thrift = new ApacheThriftSerializer();
        private static readonly MessagePackCSharpSerializer messagePack = new MessagePackCSharpSerializer();
        private static readonly CapnProtoSerializer capnproto = new CapnProtoSerializer();
        private static readonly NewtonsoftJsonSerializer newtonsoft = new NewtonsoftJsonSerializer();
        private static readonly ProtobufSerializer protobuf = new ProtobufSerializer();

        private static readonly Dictionary<string, (ISerializer serializer, short key)> Serializers = new()
        {
        { "FlatBuffers", (flatBuffers, 2) },
        { "Avro", (avro, 0) },
        { "Thrift", (thrift, 6) },
        { "MessagePack-CSharp", (messagePack, 3) },
        { "CapnProto", (capnproto, 1) },
        { "Newtonsoft.Json", (newtonsoft, 4) },
        { "Protobuf", (protobuf, 5) },
        // Adicione outros tipos e implementações aqui
    };

        public static (ISerializer serializer, short key) GetSerializer(string type)
        {
            if (Serializers.TryGetValue(type, out var serializer))
            {
                return serializer;
            }

            throw new ArgumentException("Unsupported target type");
        }
    }
}