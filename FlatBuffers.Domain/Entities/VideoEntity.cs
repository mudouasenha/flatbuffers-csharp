using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Receiver.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Entities
{
    public class VideoEntity : IFlatBufferSerializable<Video, VideoEntity>
    {
        public SocialInfoEntity SocialInfoEntity { get; set; }

        public ChannelEntity ChannelEntity { get; set; }

        public VideoInfoEntity VideoInfoEntity { get; set; }

        public VideoEntity GetFromBuffer(ByteBuffer buf)
        {
            var video = Video.GetRootAsVideo(buf);

            return FromSerializationModel(video);
        }

        public VideoEntity FromSerializationModel(Video video) => new()
        {
            SocialInfoEntity = new SocialInfoEntity().FromSerializationModel(video.SocialInfo.Value),
            ChannelEntity = new ChannelEntity().FromSerializationModel(video.Channel.Value),
            VideoInfoEntity = new VideoInfoEntity().FromSerializationModel(video.VideoInfo.Value)
        };

        public ByteBuffer CreateBuffer(FlatBufferBuilder builder, VideoEntity entity)
        {
            var channelName = builder.CreateString(entity.ChannelEntity.Name);
            var ch = Channel.CreateChannel(builder, channelName, entity.ChannelEntity.Subscribers, entity.ChannelEntity.ChannelId);

            var si = SocialInfo.CreateSocialInfo(builder, entity.SocialInfoEntity.Likes, entity.SocialInfoEntity.Dislikes, entity.SocialInfoEntity.Comments, entity.SocialInfoEntity.Views);

            var vqs = VideoInfo.CreateQualitiesVector(builder, new VideoQuality[] { VideoQuality.Lowest, VideoQuality.Low, VideoQuality.Medium, VideoQuality.SD, VideoQuality.HD, VideoQuality.TwoK, VideoQuality.FourK });
            var vi = VideoInfo.CreateVideoInfo(builder, entity.VideoInfoEntity.Duration, builder.CreateString(entity.VideoInfoEntity.Description), entity.VideoInfoEntity.Size, vqs);

            Video.StartVideo(builder);
            Video.AddSocialInfo(builder, si);
            Video.AddChannel(builder, ch);
            Video.AddVideoInfo(builder, vi);
            var video = Video.EndVideo(builder);

            builder.Finish(video.Value);

            var videoBuf = builder.DataBuffer;

            return videoBuf;
        }
    }
}