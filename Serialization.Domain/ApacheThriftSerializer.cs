using FlatBuffersModels;
using Serialization.Domain.Interfaces;
using Thrift.Protocol;
using Thrift.Transport.Client;
using Channel = Serialization.Domain.Entities.Channel;
using SocialInfo = Serialization.Domain.Entities.SocialInfo;
using Video = Serialization.Domain.Entities.Video;
using VideoInfo = Serialization.Domain.Entities.VideoInfo;

namespace Serialization.Domain
{
    public class ThriftSerializer : BaseSerializer<byte[], TBase>
    {
        #region Serialization

        protected override byte[] Serialize<T>(T original, out long messageSize) => Serialize(typeof(T), original, out messageSize);

        public override Type GetSerializationOutPutType() => typeof(byte[]);

        //protected override TBase Deserialize<T>(byte[] serializedObject) => Deserialize(typeof(T), serializedObject);


        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
        {
            var token = new CancellationToken();
            using MemoryStream memoryStream = new MemoryStream();
            using TStreamTransport transport = new TStreamTransport(memoryStream, memoryStream, new Thrift.TConfiguration());
            var protocol = new TCompactProtocol(transport);

            original.GetThriftMessage();
            var message = original.GetThriftMessage();
            message.WriteAsync(protocol);
            transport.FlushAsync(token);
            byte[] serializedData = memoryStream.ToArray();
            messageSize = serializedData.Length;
            return serializedData;
        }

        #endregion

        #region Deserialization

        protected override TBase Deserialize<T>(byte[] bytes)
        {
            var token = new CancellationToken();
            using MemoryStream memoryStream = new MemoryStream(bytes);
            using TStreamTransport transport = new TStreamTransport(memoryStream, memoryStream, new Thrift.TConfiguration());
            var protocol = new TCompactProtocol(transport);
            TBase thriftObject = (TBase)Activator.CreateInstance(typeof(T));
            thriftObject.ReadAsync(protocol, token).RunSynchronously();
            return thriftObject;
        }

        protected override TBase Deserialize(Type type, byte[] bytes)
        {
            var token = new CancellationToken();
            using MemoryStream memoryStream = new MemoryStream(bytes);
            using TStreamTransport transport = new TStreamTransport(memoryStream, memoryStream, new Thrift.TConfiguration());
            var protocol = new TCompactProtocol(transport);

            if (type == typeof(Channel))
            {
                var thriftChannel = new thriftObjects.Channel();
                thriftChannel.ReadAsync(protocol, token).RunSynchronously();
                return thriftChannel;
            }

            if (type == typeof(VideoInfo))
            {
                var thriftVideoInfo = new thriftObjects.VideoInfo();
                thriftVideoInfo.ReadAsync(protocol, token).RunSynchronously();
                return thriftVideoInfo;
            }

            if (type == typeof(SocialInfo))
            {
                var thriftSocialInfo = new thriftObjects.SocialInfo();
                thriftSocialInfo.ReadAsync(protocol, token).RunSynchronously();
                return thriftSocialInfo;
            }

            if (type == typeof(Video))
            {
                var thriftVideo = new thriftObjects.Video();
                thriftVideo.ReadAsync(protocol, token).RunSynchronously();
                return thriftVideo;
            }

            throw new NotImplementedException($"Deserialization for type {type} not implemented!");
        }

        public override bool GetDeserializationResult(Type type, out ISerializationTarget result)
        {
            var intermediateResult = DeserializationResults[type];

            if (type == typeof(Channel))
            {
                var channel = (thriftObjects.Channel)intermediateResult;
                result = new Channel()
                {
                    ChannelId = channel.ChannelId,
                    Name = channel.Name,
                    Subscribers = channel.Subscribers
                };
                return true;
            }

            if (type == typeof(VideoInfo))
            {
                var videoInfo = (thriftObjects.VideoInfo)intermediateResult;
                result = new VideoInfo()
                {
                    Duration = videoInfo.Duration,
                    Size = videoInfo.Size,
                    Description = videoInfo.Description,
                    Qualities = (VideoQualityFlatModel[])videoInfo.Qualities.ToArray().Select(x => (VideoQualityFlatModel)x)
                };
                return true;
            }

            if (type == typeof(SocialInfo))
            {
                var socialInfo = (thriftObjects.SocialInfo)intermediateResult;
                result = new SocialInfo()
                {
                    Likes = socialInfo.Likes,
                    Dislikes = socialInfo.Dislikes,
                    Views = socialInfo.Views,
                    Comments = socialInfo.Comments.ToArray()
                };
                return true;
            }

            if (type == typeof(Video))
            {
                var video = (thriftObjects.Video)intermediateResult;
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
                        Duration = video.VideoInfo.Duration,
                        Qualities = (VideoQualityFlatModel[])video.VideoInfo.Qualities.ToArray().Select(x => (VideoQualityFlatModel)x)
                    },
                    SocialInfo = new SocialInfo()
                    {
                        Likes = video.SocialInfo.Likes,
                        Dislikes = video.SocialInfo.Dislikes,
                        Views = video.SocialInfo.Views,
                        Comments = video.SocialInfo.Comments.ToArray()
                    }
                };
                return true;
            }

            throw new NotImplementedException($"Conversion for type {type} not implemented!");
        }

        #endregion

        public override string ToString()
        {
            return "Thrift";
        }

        public override bool GetSerializationResult(Type type, out object result)
        {
            throw new NotImplementedException();
        }
    }
}
