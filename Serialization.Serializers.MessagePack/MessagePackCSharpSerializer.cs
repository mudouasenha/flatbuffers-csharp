using MessagePack;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.MessagePack
{
    public class MessagePackCSharpSerializer : BaseDirectSerializer<byte[]>
    {
        #region Serialization

        protected override byte[] Serialize<T>(T original, out long messageSize)
        {
            var bytes = MessagePackSerializer.Serialize(original);
            messageSize = bytes.Length;
            return bytes;
        }

        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
        {
            var bytes = MessagePackSerializer.Serialize(type, original);
            messageSize = bytes.Length;
            return bytes;
        }

        #endregion

        #region Deserialization

        protected override ISerializationTarget Deserialize<T>(byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<T>(bytes);
        }

        protected override ISerializationTarget Deserialize(Type type, byte[] bytes)
        {
            return (ISerializationTarget)MessagePackSerializer.Deserialize(type, bytes);
        }

        #endregion

        public override string ToString() => "MessagePack-CSharp";

        public override Type GetSerializationOutPutType() => typeof(byte[]);
    }
}
