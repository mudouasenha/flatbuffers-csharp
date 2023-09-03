using Google.FlatBuffers;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.FlatBuffers
{
    public abstract class FlatBuffersConverterBase<T, Y> : IFlatBuffersSerializer<T, Y> where T : IFlatbufferObject where Y : IFlatBufferSerializable<T, Y>
    {
        public abstract Y Deserialize(byte[] buf);

        public abstract byte[] Serialize(Y entity);

        protected abstract Y GetFromBuffer(ByteBuffer buf);

        protected abstract Y FromSerializationModel(T serialized);
    }
}