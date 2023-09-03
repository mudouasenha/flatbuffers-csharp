using Serialization.Domain.FlatBuffers.VideoModel;
using Serialization.Domain.Interfaces;

namespace Serialization.Domain.Entities
{
    public class Channel : ISerializable
    {
        public string Name { get; set; }
        public int Subscribers { get; set; }
        public int ChannelId { get; set; }

        public static Channel FromSerializationModel(ChannelFlatModel serialized) => new()
        {
            Name = serialized.Name,
            Subscribers = serialized.Subscribers,
            ChannelId = serialized.ChannelId
        };

        public ISerializable Deserialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ISerializable other)
        {
            throw new NotImplementedException();
        }

        public T Serialize<T>(ISerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}