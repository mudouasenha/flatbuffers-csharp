using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Receiver.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Entities
{
    public class VideoInfoEntity : IFlatBufferSerializable<VideoInfo, VideoInfoEntity>
    {
        public int Duration { get; set; }

        public string Description { get; set; }

        public long Size { get; set; }

        public ByteBuffer CreateBuffer(FlatBufferBuilder builder, VideoInfoEntity entity)
        {
            throw new NotImplementedException();
        }

        public VideoInfoEntity FromSerializationModel(VideoInfo serialized) => new()
        {
            Duration = serialized.Duration,
            Description = serialized.Description,
            Size = serialized.Size
        };

        public VideoInfoEntity GetFromBuffer(ByteBuffer buf)
        {
            var videoInfo = VideoInfo.GetRootAsVideoInfo(buf);

            return FromSerializationModel(videoInfo);
        }
    }
}