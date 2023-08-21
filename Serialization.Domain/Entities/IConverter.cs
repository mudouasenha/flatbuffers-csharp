namespace Serialization.Domain.Entities
{
    public interface IConverter<Y>
    {
        public Y Deserialize(byte[] buf);

        public byte[] Serialize(Y entity);
    }
}