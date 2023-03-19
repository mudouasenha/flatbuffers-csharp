using FlatBuffers.Domain.Interfaces;
using Google.FlatBuffers;

namespace FlatBuffers.Sender
{
    public interface ISerializationService<T, Y> where T : IFlatbufferObject where Y : IFlatBufferSerializable<T, Y>
    {
        T Deserialize(ByteBuffer buf);

        ByteBuffer Serialize(Y entity);
    }
}