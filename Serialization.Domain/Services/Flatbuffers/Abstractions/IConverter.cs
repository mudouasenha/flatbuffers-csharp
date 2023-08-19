namespace FlatBuffers.Domain.Services.Flatbuffers.Abstractions
{
    public interface IConverter<Y>
    {
        public Y Deserialize(byte[] buf);

        public byte[] Serialize(Y entity);
    }
}