using FlatBuffers.Domain.Interfaces;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Services.Converters.Abstractions
{
    public abstract class FlatBuffersConverterBase<T, Y> : IFlatBuffersConverter<T, Y> where T : IFlatbufferObject where Y : IFlatBufferSerializable<T, Y>
    {
        public abstract Y Deserialize(byte[] buf);

        public abstract byte[] Serialize(Y entity);

        protected abstract ByteBuffer CreateBuffer(FlatBufferBuilder builder, Y entity);

        protected abstract Y GetFromBuffer(ByteBuffer buf);

        protected abstract Y FromSerializationModel(T serialized);
    }
}