namespace Serialization.Domain.Interfaces
{
    public interface ISerializer
    {
        public Y Deserialize<Y, T>(object buf) where Y : ISerializable;

        public T Serialize<Y, T>(Y entity) where Y : ISerializable;
    }
}