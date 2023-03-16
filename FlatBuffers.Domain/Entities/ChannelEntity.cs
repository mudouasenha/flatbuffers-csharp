using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Receiver.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Entities
{
    public class ChannelEntity : IFlatBufferSerializable<Channel, ChannelEntity>
    {
        public string Name { get; set; }
        public int Subscribers { get; set; }
        public int ChannelId { get; set; }

        public ByteBuffer CreateBuffer(FlatBufferBuilder builder, ChannelEntity entity)
        {
            throw new NotImplementedException();
        }

        public ChannelEntity FromSerializationModel(Channel serialized) => new()
        {
            Name = serialized.Name,
            Subscribers = serialized.Subscribers,
            ChannelId = serialized.ChannelId
        };

        public ChannelEntity GetFromBuffer(ByteBuffer buf)
        {
            var channel = Channel.GetRootAsChannel(buf);

            return FromSerializationModel(channel);
        }
    }
}