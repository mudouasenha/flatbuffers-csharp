namespace Serialization.Domain.Interfaces
{
    public interface ISerializationTarget : IEquatable<ISerializationTarget>
    {
		Type GetType();

        long Serialize(ISerializer serializer);

        //long Serialize(ref byte[] target);

        long Deserialize(ISerializer serializer);

        //long Deserialize(ref byte[] target);
    }
}