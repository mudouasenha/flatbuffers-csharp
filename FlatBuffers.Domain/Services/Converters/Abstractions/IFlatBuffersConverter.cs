using FlatBuffers.Domain.Interfaces;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Services.Converters.Abstractions
{
    public interface IFlatBuffersConverter<T, Y> : IConverter where T : IFlatbufferObject where Y : IFlatBufferSerializable<T, Y>
    {
        public abstract Y Deserialize(byte[] buf);

        public abstract byte[] Serialize(Y entity);
    }
}