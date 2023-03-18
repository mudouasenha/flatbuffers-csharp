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
            throw new NotImplementedException();
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
            var videoInfo = VideoInfoFlatModel.GetRootAsVideoInfo(buf);

            return FromSerializationModel(videoInfo);
        }
    }
}