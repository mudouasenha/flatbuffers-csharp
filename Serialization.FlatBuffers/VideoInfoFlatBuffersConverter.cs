using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.FlatBuffers.VideoModel;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.FlatBuffers
{
    public class VideoInfoFlatBuffersConverter : FlatBuffersConverterBase<VideoInfoFlatModel, VideoInfo>, IFlatBuffersVideoInfoConverter
    {
        public override VideoInfo Deserialize(byte[] byteArr)
        {
            var videoInfo = GetFromBuffer(new ByteBuffer(byteArr));

            return videoInfo;
        }

        public override byte[] Serialize(VideoInfo entity)
        {
            var builder = new FlatBufferBuilder(1024);

            VideoInfoFlatModel.StartVideoInfoFlatModel(builder);
            VideoInfoFlatModel.AddDescription(builder, builder.CreateString(entity.Description));
            VideoInfoFlatModel.AddDuration(builder, entity.Duration);
            VideoInfoFlatModel.AddSize(builder, entity.Size);
            VideoInfoFlatModel.AddQualities(builder, VideoInfoFlatModel.CreateQualitiesVector(builder, entity.Qualities));
            var videoInfoOffSet = VideoInfoFlatModel.EndVideoInfoFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);

            return builder.SizedByteArray();
        }

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