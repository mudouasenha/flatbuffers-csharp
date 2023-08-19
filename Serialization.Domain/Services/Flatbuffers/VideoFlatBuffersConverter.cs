using BenchmarkDotNet.Attributes;
using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Flatbuffers.Abstractions;
using FlatBuffers.Domain.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Services.Flatbuffers
{
    public partial class VideoFlatBuffersConverter : FlatBuffersConverterBase<VideoFlatModel, Video>, IFlatBuffersVideoConverter
    {
        public override Video Deserialize(byte[] byteArr)
        {
            var video = GetFromBuffer(new ByteBuffer(byteArr));

            return video;
        }

        [Benchmark]
        public override byte[] Serialize(Video entity)
        {
            var builder = new FlatBufferBuilder(1024);
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

            var bArr = builder.SizedByteArray();

            return bArr;
        }

        [Benchmark]
        protected VideoFlatModel DeserializeFlatModel(ByteBuffer buf) => VideoFlatModel.GetRootAsVideoFlatModel(buf);

        protected override Video FromSerializationModel(VideoFlatModel video) => new()
        {
            SocialInfo = SocialInfo.FromSerializationModel(video.SocialInfo.Value),
            Channel = Channel.FromSerializationModel(video.Channel.Value),
            VideoInfo = VideoInfo.FromSerializationModel(video.VideoInfo.Value)
        };

        protected override Video GetFromBuffer(ByteBuffer buf)
        {
            var video = DeserializeFlatModel(buf);

            return FromSerializationModel(video);
        }
    }
}