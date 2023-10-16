using Google.Protobuf;

namespace Serialization.Domain.Interfaces
{
    public interface ISerializationTarget : IEquatable<ISerializationTarget>
    {
        Type GetType();

        long Serialize(ISerializer serializer);

        long Deserialize(ISerializer serializer);

        void CreateProtobufMessage();

        IMessage GetProtobufMessage();
    }
}