using BenchmarkDotNet.Attributes;
using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Converters.Abstractions;
using FlatBuffers.Domain.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Services.Converters
{
    public class ChannelFlatBuffersConverter : FlatBuffersConverterBase<ChannelFlatModel, Channel>, IFlatBuffersChannelConverter
    {
        private readonly FlatBufferBuilder _flatBufferBuilder;

        public ChannelFlatBuffersConverter() => _flatBufferBuilder = new FlatBufferBuilder(1024);

        public override Channel Deserialize(byte[] byteArr)
        {
            var channel = GetFromBuffer(new ByteBuffer(byteArr));

            return channel;
        }

        public override byte[] Serialize(Channel channel)
        {
            var buf = CreateBuffer(_flatBufferBuilder, channel);

            return buf.ToFullArray();
        }

        [Benchmark]
        protected override ByteBuffer CreateBuffer(FlatBufferBuilder builder, Channel entity)
        {
            ChannelFlatModel.StartChannelFlatModel(builder);
            ChannelFlatModel.AddName(builder, builder.CreateString(entity.Name));
            ChannelFlatModel.AddSubscribers(builder, entity.Subscribers);
            ChannelFlatModel.AddChannelId(builder, entity.ChannelId);
            var videoInfoOffSet = ChannelFlatModel.EndChannelFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);

            return builder.DataBuffer;
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