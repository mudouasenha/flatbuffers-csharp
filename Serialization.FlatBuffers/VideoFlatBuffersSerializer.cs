using FlatBuffersModels;
using Google.FlatBuffers;
using Serialization.Domain.Entities;

namespace Serialization.Serializers.FlatBuffers
{
    public class VideoFlatBuffersSerializer
    {
        internal static FlatBufferBuilder builder = new(1);

        public static byte[] Serialize(Video entity, out long messageSize)
        {
            var builder = new FlatBufferBuilder(1024);
            var stringOffSets = entity.SocialInfo.Comments.Select(c => builder.CreateString(c)).ToArray();
            var channelName = builder.CreateString(entity.Channel.Name);
            var videoInfoDescription = builder.CreateString(entity.VideoInfo.Description);
            var socialInfoComments = SocialInfoFlatModel.CreateCommentsVector(builder, stringOffSets);

            var ch = ChannelFlatModel.CreateChannelFlatModel(builder, channelName, entity.Channel.Subscribers, entity.Channel.ChannelId);
            var si = SocialInfoFlatModel.CreateSocialInfoFlatModel(builder, entity.SocialInfo.Likes, entity.SocialInfo.Dislikes, socialInfoComments, entity.SocialInfo.Views);
            var vqs = VideoInfoFlatModel.CreateQualitiesVector(builder, entity.VideoInfo.Qualities);
            var vi = VideoInfoFlatModel.CreateVideoInfoFlatModel(builder, entity.VideoInfo.Duration, videoInfoDescription, entity.VideoInfo.Size, vqs);

            VideoFlatModel.StartVideoFlatModel(builder);
            VideoFlatModel.AddSocialInfo(builder, si);
            VideoFlatModel.AddChannel(builder, ch);
            VideoFlatModel.AddVideoInfo(builder, vi);

            var video = VideoFlatModel.EndVideoFlatModel(builder);

            builder.Finish(video.Value);

            messageSize = FlatBuffersSerializer.GetSize();

            var byteArray = builder.SizedByteArray();
            builder.Clear();
            return byteArray;
        }

        public static Video FromSerializationModel(VideoFlatModel video) =>
            new(SocialInfoFlatBuffersSerializer.FromSerializationModel(video.SocialInfo.Value), ChannelFlatBuffersSerializer.FromSerializationModel(video.Channel.Value), VideoInfoFlatBuffersSerializer.FromSerializationModel(video.VideoInfo.Value));
    }
}