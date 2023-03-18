using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Domain.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Entities
{
    public class Channel : IFlatBufferSerializable<ChannelFlatModel, Channel>
    {
        public string Name { get; set; }
        public int Subscribers { get; set; }
        public int ChannelId { get; set; }

        public ByteBuffer CreateBuffer(FlatBufferBuilder builder, Channel entity)
        {
            throw new NotImplementedException();
        }

        public Channel FromSerializationModel(ChannelFlatModel serialized) => new()
        {
            Name = serialized.Name,
            Subscribers = serialized.Subscribers,
            ChannelId = serialized.ChannelId
        };

        public Channel GetFromBuffer(ByteBuffer buf)
        {
            var channel = ChannelFlatModel.GetRootAsChannel(buf);

            return FromSerializationModel(channel);
        }
    }
}