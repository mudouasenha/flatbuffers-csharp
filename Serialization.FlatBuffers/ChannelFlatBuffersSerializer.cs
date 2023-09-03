using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.FlatBuffers.VideoModel;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.FlatBuffers
{
    public class ChannelFlatBuffersSerializer : FlatBuffersSerializerBase<ChannelFlatModel, Channel>, IFlatBuffersChannelSerializer
    {
        public override Channel Deserialize(byte[] byteArr)
        {
            var channel = GetFromBuffer(new ByteBuffer(byteArr));

            return channel;
        }

        public override byte[] Serialize(Channel entity)
        {
            var builder = new FlatBufferBuilder(1024);

            ChannelFlatModel.StartChannelFlatModel(builder);
            ChannelFlatModel.AddName(builder, builder.CreateString(entity.Name));
            ChannelFlatModel.AddSubscribers(builder, entity.Subscribers);
            ChannelFlatModel.AddChannelId(builder, entity.ChannelId);
            var videoInfoOffSet = ChannelFlatModel.EndChannelFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);

            return builder.SizedByteArray();
        }

        protected ChannelFlatModel DeserializeFlatModel(ByteBuffer buf) => ChannelFlatModel.GetRootAsChannelFlatModel(buf);

        protected override Channel FromSerializationModel(ChannelFlatModel serialized) => new()
        {
            Name = serialized.Name,
            Subscribers = serialized.Subscribers,
            ChannelId = serialized.ChannelId
        };

        protected override Channel GetFromBuffer(ByteBuffer buf)
        {
            var channel = DeserializeFlatModel(buf);

            return FromSerializationModel(channel);
        }
    }
}