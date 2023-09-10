using FlatBuffersModels;
using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.FlatBuffers
{
    public class VideoFlatBuffersSerializer : FlatBuffersSerializerBase<byte[], Video, VideoFlatModel>
    {
        internal static FlatBufferBuilder builder = new(1);

        public override bool GetDeserializationResult(Type type, out ISerializationTarget result)
        {
            if (type == typeof(Video))
            {
                result = FromSerializationModel((VideoFlatModel)DeserializationResults[typeof(VideoFlatModel)]);
                return true;
            }
            throw new NotImplementedException($"Conversion for type {type} not implemented!");
        }

        public override bool GetSerializationResult(Type type, out object result)
        {
            if (type == typeof(Video))
            {
                result = SerializationResults[typeof(Video)].Result;
                return true;
            }
            throw new NotImplementedException($"Conversion for type {type} not implemented!");
        }

        protected override IFlatbufferObject Deserialize(Type type, byte[] serializedObject)
        {
            if (type == typeof(Video))
            {
                var buf = new ByteBuffer(serializedObject);
                var offset = buf.GetInt(buf.Position) + buf.Position;
                var video = new VideoFlatModel().__assign(offset, buf);
                return video;
            }
            throw new NotImplementedException($"Deserialization for type {type} not implemented!");
        }

        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize) =>
            type == typeof(Video) ? Serialize((Video)original, out messageSize) : throw new NotImplementedException($"Serialization for type {type} not implemented!");

        private static byte[] Serialize(Video entity, out long messageSize)
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

            messageSize = GetSize();

            var byteArray = builder.SizedByteArray();
            builder.Clear();
            return byteArray;
        }

        protected static Video FromSerializationModel(VideoFlatModel video) =>
            new(SocialInfoFlatBuffersSerializer.FromSerializationModel(video.SocialInfo.Value), ChannelFlatBuffersSerializer.FromSerializationModel(video.Channel.Value), VideoInfoFlatBuffersSerializer.FromSerializationModel(video.VideoInfo.Value));
    }
}