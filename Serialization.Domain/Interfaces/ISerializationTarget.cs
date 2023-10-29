using Avro.Specific;
using Google.Protobuf;
using Thrift.Protocol;

namespace Serialization.Domain.Interfaces
{
    public interface ISerializationTarget : IEquatable<ISerializationTarget>
    {
        Type GetType();

        long Serialize(ISerializer serializer);

        long Deserialize(ISerializer serializer);

        void CreateProtobufMessage();

        IMessage GetProtobufMessage();

        TBase GetThriftMessage();

        void CreateThriftMessage();

        ISpecificRecord GetAvroMessage();

        void CreateAvroMessage();
    }
}