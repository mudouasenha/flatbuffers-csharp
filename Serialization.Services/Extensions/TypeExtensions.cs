using Serialization.Domain;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.ApacheAvro;
using Serialization.Serializers.CapnProto;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.MessagePack;
using Serialization.Serializers.SystemTextJson;

namespace Serialization.Services.Extensions
{
    public static class TypeExtensions
    {
        public static ISerializer GetSerializer(this string type)
        {
            if (type == "FlatBuffers") return new FlatBuffersSerializer();
            if (type == "Avro") return new ApacheAvroSerializer();
            if (type == "Thrift") return new ApacheThriftSerializer();
            if (type == "MessagePack-CSharp") return new MessagePackCSharpSerializer();
            if (type == "CapnProto") return new CapnProtoSerializer();
            if (type == "Newtonsoft.Json") return new NewtonsoftJsonSerializer();
            if (type == "Protobuf") return new ProtobufSerializer();
            //if (typeToCheck == "BinaryFormatterSerializer") return new BinaryFormatterSerializer();

            throw new ArgumentException("Unsupported target type");
        }

        public static Type GetTargetType(this string type)
        {
            if (type == "Channel") return typeof(Channel);
            if (type == "SocialInfo") return typeof(SocialInfo);
            if (type == "VideoInfo") return typeof(VideoInfo);
            if (type == "Video") return typeof(Video);

            throw new ArgumentException("Unsupported target type");
        }
    }
}