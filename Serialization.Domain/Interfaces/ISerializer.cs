namespace Serialization.Domain.Interfaces
{
    public interface ISerializer<Y>
    {
        public Y Deserialize(byte[] buf);

        public byte[] Serialize(Y entity);

        protected abstract Y Serialize<T>(T original, out long messageSize);

        protected abstract byte[] Deserialize<T>(T serializedObject) where T : ISerializable;
    }
}