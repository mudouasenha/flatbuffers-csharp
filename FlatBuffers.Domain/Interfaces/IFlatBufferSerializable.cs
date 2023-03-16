using Google.FlatBuffers;

namespace FlatBuffers.Domain.Interfaces
{
    public interface IFlatBufferSerializable<T, Y> where T : IFlatbufferObject where Y : IFlatBufferSerializable<T, Y>
    {
        abstract Y GetFromBuffer(ByteBuffer buf);

        abstract ByteBuffer CreateBuffer(FlatBufferBuilder builder, Y entity);

        abstract Y FromSerializationModel(T serialized);
    }
}