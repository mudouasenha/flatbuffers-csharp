using BenchmarkDotNet.Attributes;
using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Converters.Abstractions;
using FlatBuffers.Domain.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Services.Converters
{
    public class VideoInfoFlatBuffersConverter : FlatBuffersConverterBase<VideoInfoFlatModel, VideoInfo>, IFlatBuffersVideoInfoConverter
    {
        private readonly FlatBufferBuilder _flatBufferBuilder;

        public VideoInfoFlatBuffersConverter() => _flatBufferBuilder = new FlatBufferBuilder(1024);

        public override VideoInfo Deserialize(byte[] byteArr)
        {
            var videoInfo = GetFromBuffer(new ByteBuffer(byteArr));

            return videoInfo;
        }

        public override byte[] Serialize(VideoInfo videoInfo)
        {
            var buf = CreateBuffer(_flatBufferBuilder, videoInfo);

            return buf.ToSizedArray();
        }

        [Benchmark]
        protected override ByteBuffer CreateBuffer(FlatBufferBuilder builder, VideoInfo entity)
        {
            VideoInfoFlatModel.StartVideoInfoFlatModel(builder);
            VideoInfoFlatModel.AddDescription(builder, builder.CreateString(entity.Description));
            VideoInfoFlatModel.AddDuration(builder, entity.Duration);
            VideoInfoFlatModel.AddSize(builder, entity.Size);
            VideoInfoFlatModel.AddQualities(builder, VideoInfoFlatModel.CreateQualitiesVector(builder, entity.Qualities));
            var videoInfoOffSet = VideoInfoFlatModel.EndVideoInfoFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);

            return builder.DataBuffer;
        }

        [Benchmark]
        protected VideoInfoFlatModel DeserializeFlatModel(ByteBuffer buf) => VideoInfoFlatModel.GetRootAsVideoInfoFlatModel(buf);

        protected override VideoInfo FromSerializationModel(VideoInfoFlatModel serialized) => new()
        {
            Duration = serialized.Duration,
            Description = serialized.Description,
            Size = serialized.Size,
            Qualities = serialized.GetQualitiesArray()
        };

        protected override VideoInfo GetFromBuffer(ByteBuffer buf)
        {
            var videoInfo = DeserializeFlatModel(buf);

            return FromSerializationModel(videoInfo);
        }
    }
}