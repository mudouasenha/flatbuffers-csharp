namespace FlatBuffers.Domain.Services.Converters.Abstractions
{
    public interface IConverter<Y>
    {
        public Y Deserialize(byte[] buf);

        public byte[] Serialize(Y entity);
    }
}