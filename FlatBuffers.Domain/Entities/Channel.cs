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
            ChannelFlatModel.StartChannelFlatModel(builder);
            ChannelFlatModel.AddName(builder, builder.CreateString(entity.Name));
            ChannelFlatModel.AddSubscribers(builder, entity.Subscribers);
            ChannelFlatModel.AddChannelId(builder, entity.ChannelId);
            var videoInfoOffSet = ChannelFlatModel.EndChannelFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);

            return builder.DataBuffer;
        }

        public Channel FromSerializationModel(ChannelFlatModel serialized) => new()
        {
            Name = serialized.Name,
            Subscribers = serialized.Subscribers,
            ChannelId = serialized.ChannelId
        };

        public Channel GetFromBuffer(ByteBuffer buf)
        {
            var channel = ChannelFlatModel.GetRootAsChannelFlatModel(buf);

            return FromSerializationModel(channel);
        }
    }
}