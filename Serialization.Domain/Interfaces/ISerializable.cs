namespace Serialization.Domain.Interfaces
{
    public interface ISerializable : IEquatable<ISerializable>
    {
        ISerializable Deserialize(ISerializer serializer);

        T Serialize<T>(ISerializer serializer);

        Type GetType();
    }
}