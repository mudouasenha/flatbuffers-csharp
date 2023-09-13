using FlatBuffersModels;
using Google.FlatBuffers;
using Serialization.Domain.Entities;

namespace Serialization.Serializers.FlatBuffers
{
    public class ChannelFlatBuffersSerializer
    {
        public static Channel FromSerializationModel(ChannelFlatModel serialized) => new(serialized.Name, serialized.Subscribers, serialized.ChannelId);

        public static byte[] Serialize(Channel entity, out long messageSize)
        {
            var builder = new FlatBufferBuilder(1024);
            var name = builder.CreateString(entity.Name);

            ChannelFlatModel.StartChannelFlatModel(builder);
            ChannelFlatModel.AddName(builder, name);
            ChannelFlatModel.AddSubscribers(builder, entity.Subscribers);
            ChannelFlatModel.AddChannelId(builder, entity.ChannelId);

            var videoInfoOffSet = ChannelFlatModel.EndChannelFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);

            messageSize = FlatBuffersSerializer.GetSize();

            var byteArray = builder.SizedByteArray();
            builder.Clear();
            return byteArray;
        }
    }
}