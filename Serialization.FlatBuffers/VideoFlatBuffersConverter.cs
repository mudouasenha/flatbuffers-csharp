using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.FlatBuffers.VideoModel;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.FlatBuffers
{
    public partial class VideoFlatBuffersConverter : FlatBuffersConverterBase<VideoFlatModel, Video>, IFlatBuffersVideoConverter
    {
        public override Video Deserialize(byte[] byteArr)
        {
            var video = GetFromBuffer(new ByteBuffer(byteArr));

            return video;
        }

        public override byte[] Serialize(Video entity)
        {
            var builder = new FlatBufferBuilder(1024);
            var channelName = builder.CreateString(entity.Channel.Name);
            var ch = ChannelFlatModel.CreateChannelFlatModel(builder, channelName, entity.Channel.Subscribers, entity.Channel.ChannelId);


            // TODO: Ajustar, é necessário compilar novamente FlatModels 
            var si = SocialInfoFlatModel.CreateSocialInfoFlatModel(builder, entity.SocialInfo.Likes, entity.SocialInfo.Dislikes, entity.SocialInfo.Comments, entity.SocialInfo.Views);
            //var si = SocialInfoFlatModel.CreateSocialInfoFlatModel(builder, entity.SocialInfo.Likes, entity.SocialInfo.Dislikes, entity.SocialInfo.CommentsCount, entity.SocialInfo.Views);

            var vqs = VideoInfoFlatModel.CreateQualitiesVector(builder, entity.VideoInfo.Qualities);
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