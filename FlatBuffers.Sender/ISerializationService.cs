using FlatBuffers.Domain.Interfaces;
using Google.FlatBuffers;

namespace FlatBuffers.Sender
{
    public interface ISerializationService<T, Y> where T : IFlatbufferObject where Y : IFlatBufferSerializable
    {
        ByteBuffer Serialize(Y entity);

        T Deserialize(ByteBuffer buf);
    }
}