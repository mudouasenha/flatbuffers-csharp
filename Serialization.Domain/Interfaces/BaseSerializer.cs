namespace Serialization.Domain.Interfaces
{
    public abstract class BaseSerializer<ISerializationObject, IDeserializationObject> : ISerializer
    {
        protected abstract ISerializationObject Serialize<T>(T original, out long messageSize) where T : ISerializationObject;
        protected abstract ISerializationObject Serialize(Type type, ISerializationObject original, out long messageSize);
        protected abstract IDeserializationObject Deserialize<T>(ISerializationObject serializedObject) where T : ISerializationObject;
        protected abstract IDeserializationObject Deserialize(Type type, ISerializationObject serializedObject);

        public Y Deserialize<Y, T>(object buf) where Y : Interfaces.ISerializable
        {
            throw new NotImplementedException();
        }

        public T Serialize<Y, T>(Y entity) where Y : Interfaces.ISerializable
        {
            throw new NotImplementedException();
        }
    }

    public abstract class BaseDirectSerializer<TSerialization> : BaseSerializer<TSerialization, ISerializable>
    {
    }
}
