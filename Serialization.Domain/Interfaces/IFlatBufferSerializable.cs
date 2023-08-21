using Google.FlatBuffers;

namespace Serialization.Domain.Interfaces
{
    public interface IFlatBufferSerializable<T, Y> : ISerializable where T : IFlatbufferObject where Y : IFlatBufferSerializable<T, Y>
    {
    }
}