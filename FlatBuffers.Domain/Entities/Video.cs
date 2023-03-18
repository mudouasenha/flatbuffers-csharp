using BenchmarkDotNet.Attributes;
using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Domain.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Entities
{
    [MemoryDiagnoser]
    [CsvExporter]
    public class Video : IFlatBufferSerializable<VideoFlatModel, Video>
    {
        public SocialInfo SocialInfo { get; set; }

        public Channel Channel { get; set; }

        public VideoInfo VideoInfo { get; set; }

        public Video GetFromBuffer(ByteBuffer buf)
        {
            var video = Deserialize(buf);

            return FromSerializationModel(video);
        }

        [Benchmark]
        private VideoFlatModel Deserialize(ByteBuffer buf) => VideoFlatModel.GetRootAsVideoFlatModel(buf);

        public Video FromSerializationModel(VideoFlatModel video) => new()
        {
            SocialInfo = new SocialInfo().FromSerializationModel(video.SocialInfo.Value),
            Channel = new Channel().FromSerializationModel(video.Channel.Value),
            VideoInfo = new VideoInfo().FromSerializationModel(video.VideoInfo.Value)
        };

        [Benchmark]
        public ByteBuffer CreateBuffer(FlatBufferBuilder builder, Video entity)
        {
            var channelName = builder.CreateString(entity.Channel.Name);
            var ch = ChannelFlatModel.CreateChannelFlatModel(builder, channelName, entity.Channel.Subscribers, entity.Channel.ChannelId);

            var si = SocialInfoFlatModel.CreateSocialInfoFlatModel(builder, entity.SocialInfo.Likes, entity.SocialInfo.Dislikes, entity.SocialInfo.Comments, entity.SocialInfo.Views);

            var vqs = VideoInfoFlatModel.CreateQualitiesVector(builder, new VideoQualityFlatModel[] { VideoQualityFlatModel.Lowest, VideoQualityFlatModel.Low, VideoQualityFlatModel.Medium, VideoQualityFlatModel.SD, VideoQualityFlatModel.HD, VideoQualityFlatModel.TwoK, VideoQualityFlatModel.FourK });
            var vi = VideoInfoFlatModel.CreateVideoInfoFlatModel(builder, entity.VideoInfo.Duration, builder.CreateString(entity.VideoInfo.Description), entity.VideoInfo.Size, vqs);

            VideoFlatModel.StartVideoFlatModel(builder);
            VideoFlatModel.AddSocialInfo(builder, si);
            VideoFlatModel.AddChannel(builder, ch);
            VideoFlatModel.AddVideoInfo(builder, vi);
            var video = VideoFlatModel.EndVideoFlatModel(builder);

            builder.Finish(video.Value);

            return builder.DataBuffer;
        }
    }
}