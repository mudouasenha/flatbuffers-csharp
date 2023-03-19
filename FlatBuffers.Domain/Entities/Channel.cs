using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Domain.VideoModel;

namespace FlatBuffers.Domain.Entities
{
    public class Channel : IFlatBufferSerializable<ChannelFlatModel, Channel>
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
    }
}