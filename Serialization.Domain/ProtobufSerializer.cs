using Google.Protobuf;
using Serialization.Domain.Entities.Enums;
using Serialization.Domain.Interfaces;
using Channel = Serialization.Domain.Entities.Channel;
using SocialInfo = Serialization.Domain.Entities.SocialInfo;
using Video = Serialization.Domain.Entities.Video;
using VideoInfo = Serialization.Domain.Entities.VideoInfo;

namespace Serialization.Domain
{
    public class ProtobufSerializer : BaseSerializer<byte[], IMessage>
    {
        #region Serialization

        protected override byte[] Serialize<T>(T original, out long messageSize)
        {
            return Serialize(typeof(T), original, out messageSize);
        }

        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
        {
            original.CreateProtobufMessage();
            var message = original.GetProtobufMessage();
            messageSize = message.CalculateSize();
            var bytes = new byte[messageSize];
            message.WriteTo(bytes);
            return bytes;
        }

        #endregion Serialization

        #region Deserialization

        protected override IMessage Deserialize<T>(byte[] bytes)
        {
            return Deserialize(typeof(T), bytes);
        }

        protected override IMessage Deserialize(Type type, byte[] bytes)
        {
            if (type == typeof(Channel))
            {
                var person = ProtoObjects.Channel.Parser.ParseFrom(bytes);
                return person;
            }

            if (type == typeof(VideoInfo))
            {
                var vector3 = ProtoObjects.VideoInfo.Parser.ParseFrom(bytes);
                return vector3;
            }

            if (type == typeof(SocialInfo))
            {
                var vector3 = ProtoObjects.SocialInfo.Parser.ParseFrom(bytes);
                return vector3;
            }

            if (type == typeof(Video))
            {
                var vector3 = ProtoObjects.Video.Parser.ParseFrom(bytes);
                return vector3;
            }

            throw new NotImplementedException($"Deserialization for type {type} not implemented!");
        }

        #endregion Deserialization

        public override bool GetDeserializationResult(Type type, out ISerializationTarget result)
        {
            var intermediateResult = DeserializationResults[type];

            if (type == typeof(Channel))
            {
                var channel = (ProtoObjects.Channel)intermediateResult;
                result = new Channel()
                {
                    ChannelId = channel.ChannelId,
                    Name = channel.Name,
                    Subscribers = (int)channel.Subscribers
                };
                return true;
            }

            if (type == typeof(VideoInfo))
            {
                var videoInfo = (ProtoObjects.VideoInfo)intermediateResult;
                result = new VideoInfo()
                {
                    Duration = (long)videoInfo.Duration,
                    Size = (long)videoInfo.Size,
                    Description = videoInfo.Description,
                    Qualities = (VideoQualities[])videoInfo.Qualities.ToArray().Select(x => (VideoQualities)x)
                };
                return true;
            }

            if (type == typeof(SocialInfo))
            {
                var socialInfo = (ProtoObjects.SocialInfo)intermediateResult;
                result = new SocialInfo()
                {
                    Likes = (int)socialInfo.Likes,
                    Dislikes = (int)socialInfo.Dislikes,
                    Views = (int)socialInfo.Views,
                    Comments = socialInfo.Comments.ToArray()
                };
                return true;
            }

            if (type == typeof(Video))
            {
                var video = (ProtoObjects.Video)intermediateResult;
                result = new Video()
                {
                    VideoId = video.VideoId,
                    Url = video.Url,
                    Channel = new Channel()
                    {
                        ChannelId = video.Channel.ChannelId,
                        Name = video.Channel.Name,
                        Subscribers = (int)video.Channel.Subscribers
                    },
                    VideoInfo = new VideoInfo()
                    {
                        Description = video.VideoInfo.Description,
                        Duration = (long)video.VideoInfo.Duration,
                        Qualities = (VideoQualities[])video.VideoInfo.Qualities.ToArray().Select(x => (VideoQualities)x)
                    },
                    SocialInfo = new SocialInfo()
                    {
                        Likes = (int)video.SocialInfo.Likes,
                        Dislikes = (int)video.SocialInfo.Dislikes,
                        Views = (int)video.SocialInfo.Views,
                        Comments = video.SocialInfo.Comments.ToArray()
                    }
                };
                return true;
            }

            throw new NotImplementedException($"Conversion for type {type} not implemented!");
        }

        public override string ToString() => "Protobuf";

        public override Type GetSerializationOutPutType() => typeof(byte[]);
    }
}