using BenchmarkDotNet.Attributes;
using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Converters.Abstractions;
using FlatBuffers.Domain.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Services.Converters
{
    public class ChannelFlatBuffersConverter : FlatBuffersConverterBase<ChannelFlatModel, Channel>, IFlatBuffersChannelConverter
    {
        public override Channel Deserialize(byte[] byteArr)
        {
            var channel = GetFromBuffer(new ByteBuffer(byteArr));

            return channel;
        }

        [Benchmark]
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

        [Benchmark]
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