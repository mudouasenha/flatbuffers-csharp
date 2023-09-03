namespace Serialization.Domain.Interfaces
{
    public interface ISerializable : IEquatable<ISerializable>
    {
        long Deserialize(ISerializer<ISerializable> serializer);

        long Serialize(ISerializer<ISerializable> serializer);

        Type GetType();
    }
}