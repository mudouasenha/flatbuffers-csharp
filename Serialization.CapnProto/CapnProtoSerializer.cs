using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.CapnProto
{
    public class CapnProtoSerializer : BaseDirectSerializer<byte[]>
    {
        public override Type GetSerializationOutPutType()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return "CapnProto";
        }

        protected override ISerializationTarget Deserialize<T>(byte[] serializedObject)
        {
            throw new NotImplementedException();
        }

        protected override ISerializationTarget Deserialize(Type type, byte[] serializedObject)
        {
            throw new NotImplementedException();
        }

        protected override byte[] Serialize<T>(T original, out long messageSize)
        {
            throw new NotImplementedException();
        }

        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
        {
            throw new NotImplementedException();
        }
    }
}
