using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Abstractions;
using FlatBuffers.Receiver.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Sender
{
    public partial class SerializationService : IVideoSerializationService
    {
        public VideoFlatModel Deserialize(ByteBuffer buf)
        {
            var video = VideoFlatModel.GetRootAsVideo(buf);

            return video;
        }

        public ByteBuffer Serialize(Video videoModel)
        {
            var builder = new FlatBufferBuilder(1024);
            return videoModel.CreateBuffer(builder, videoModel);

            /*var channelName = _flatBufferBuilder.CreateString("SalveDrew");
            var ch = Channel.CreateChannel(_flatBufferBuilder, channelName, 7000, 15);

            var si = SocialInfo.CreateSocialInfo(_flatBufferBuilder, 1_000, 1, 35, 25_000);

            var vqs = VideoInfo.CreateQualitiesVector(_flatBufferBuilder, new VideoQuality[] { VideoQuality.Lowest, VideoQuality.Low, VideoQuality.Medium, VideoQuality.SD, VideoQuality.HD, VideoQuality.TwoK, VideoQuality.FourK });
            var vi = VideoInfo.CreateVideoInfo(_flatBufferBuilder, 300, _flatBufferBuilder.CreateString("A vidaé dura, mas torcer parao corinthians é uma delícia!"), 1_000_000_000, vqs);

            Video.StartVideo(_flatBufferBuilder);
            Video.AddSocialInfo(_flatBufferBuilder, si);
            Video.AddChannel(_flatBufferBuilder, ch);
            Video.AddVideoInfo(_flatBufferBuilder, vi);
            var video = Video.EndVideo(_flatBufferBuilder);

            _flatBufferBuilder.Finish(video.Value);

            var videoBuf = _flatBufferBuilder.DataBuffer;

            return videoBuf;*/
        }
    }
}