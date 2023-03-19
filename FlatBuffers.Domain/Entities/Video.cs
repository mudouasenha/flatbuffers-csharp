using BenchmarkDotNet.Attributes;
using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Domain.VideoModel;

namespace FlatBuffers.Domain.Entities
{
    [MemoryDiagnoser]
    [CsvExporter]
    public class Video : IFlatBufferSerializable<VideoFlatModel, Video>
    {
        public SocialInfo SocialInfo { get; set; }

        public Channel Channel { get; set; }

        public VideoInfo VideoInfo { get; set; }

        public static Video FromSerializationModel(VideoFlatModel video) => new()
        {
            SocialInfo = SocialInfo.FromSerializationModel(video.SocialInfo.Value),
            Channel = Channel.FromSerializationModel(video.Channel.Value),
            VideoInfo = VideoInfo.FromSerializationModel(video.VideoInfo.Value)
        };
    }
}