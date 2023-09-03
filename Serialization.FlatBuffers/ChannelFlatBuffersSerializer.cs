using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.FlatBuffers.VideoModel;

namespace Serialization.Serializers.FlatBuffers
{
    public class ChannelFlatBuffersSerializer : FlatBuffersSerializerBase<byte[], ChannelFlatModel>
    {
        public Channel Deserialize(byte[] byteArr)
        {
            var channel = GetFromBuffer(new ByteBuffer(byteArr));

            return channel;
        }

        public byte[] Serialize(Channel entity)
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

        protected override IFlatbufferObject Deserialize(Type type, byte[] serializedObject)
        {
            throw new NotImplementedException();
        }

        protected ChannelFlatModel DeserializeFlatModel(ByteBuffer buf) => ChannelFlatModel.GetRootAsChannelFlatModel(buf);

        protected Channel FromSerializationModel(ChannelFlatModel serialized) => new()
        {
            Name = serialized.Name,
            Subscribers = serialized.Subscribers,
            ChannelId = serialized.ChannelId
        };

        protected Channel GetFromBuffer(ByteBuffer buf)
        {
            var channel = DeserializeFlatModel(buf);

            return FromSerializationModel(channel);
        }

        protected override byte[] Serialize(Type type, byte[] original, out long messageSize)
        {
            throw new NotImplementedException();
        }
    }
}