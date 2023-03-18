using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Domain.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Entities
{
    public class VideoInfo : IFlatBufferSerializable<VideoInfoFlatModel, VideoInfo>
    {
        public int Duration { get; set; }

        public string Description { get; set; }

        public long Size { get; set; }

        public VideoQualityFlatModel[] Qualities { get; set; }

        public ByteBuffer CreateBuffer(FlatBufferBuilder builder, VideoInfo entity)
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

        public VideoInfo FromSerializationModel(VideoInfoFlatModel serialized) => new()
        {
            Duration = serialized.Duration,
            Description = serialized.Description,
            Size = serialized.Size,
            Qualities = serialized.GetQualitiesArray()
        };

        public VideoInfo GetFromBuffer(ByteBuffer buf)
        {
            var videoInfo = VideoInfoFlatModel.GetRootAsVideoInfoFlatModel(buf);

            return FromSerializationModel(videoInfo);
        }
    }
}