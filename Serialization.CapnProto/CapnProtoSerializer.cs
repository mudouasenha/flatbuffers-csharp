using Capnp;
using Serialization.Domain.Entities;
using Serialization.Domain.Entities.Enums;
using Serialization.Domain.Interfaces;
using static CapnpGen.VideoInfo;

namespace Serialization.Serializers.CapnProto
{
    public class CapnProtoSerializer : BaseSerializer<byte[], ICapnpSerializable>
    {
        protected override byte[] Serialize<T>(T original, out long messageSize) => Serialize(typeof(T), original, out messageSize);

        protected override ICapnpSerializable Deserialize<T>(byte[] serializedObject) => Deserialize(typeof(T), serializedObject);

        protected override ICapnpSerializable Deserialize(Type type, byte[] serializedObject)
        {
            using (MemoryStream stream = new MemoryStream(serializedObject))
            {
                var frame = Framing.ReadSegments(stream);

                var deserializer = DeserializerState.CreateRoot(frame);
                if (type == typeof(Channel))
                {
                    var reader = new CapnpGen.Channel.READER(deserializer);
                    var msg = new CapnpGen.Channel()
                    {
                        ChannelId = reader.ChannelId,
                        Name = reader.Name,
                        Subscribers = reader.Subscribers,
                    };
                    return msg;
                }
                if (type == typeof(VideoInfo))
                {
                    var reader = new CapnpGen.VideoInfo.READER(deserializer);
                    var msg = new CapnpGen.VideoInfo()
                    {
                        Description = reader.Description,
                        Duration = reader.Duration,
                        Qualities = reader.Qualities,
                        Size = reader.Size,
                    };
                    return msg;
                }
                if (type == typeof(SocialInfo))
                {
                    var reader = new CapnpGen.SocialInfo.READER(deserializer);
                    var msg = new CapnpGen.SocialInfo()
                    {
                        Comments = reader.Comments,
                        Dislikes = reader.Dislikes,
                        Likes = reader.Likes,
                        Views = reader.Views,
                    };
                    return msg;
                }
                if (type == typeof(Video))
                {
                    var reader = new CapnpGen.Video.READER(deserializer);
                    var msg = new CapnpGen.Video()
                    {
                        Url = reader.Url,
                        VideoId = reader.VideoId,
                        Channel = new CapnpGen.Channel()
                        {
                            ChannelId = reader.Channel.ChannelId,
                            Name = reader.Channel.Name,
                            Subscribers = reader.Channel.Subscribers,
                        },
                        SocialInfo = new CapnpGen.SocialInfo()
                        {
                            Comments = reader.SocialInfo.Comments,
                            Dislikes = reader.SocialInfo.Dislikes,
                            Likes = reader.SocialInfo.Likes,
                            Views = reader.SocialInfo.Views,
                        },
                        VideoInfo = new CapnpGen.VideoInfo()
                        {
                            Description = reader.VideoInfo.Description,
                            Duration = reader.VideoInfo.Duration,
                            Qualities = reader.VideoInfo.Qualities,
                            Size = reader.VideoInfo.Size,
                        }
                    };
                    return msg;
                }
            }

            throw new NotImplementedException();
        }

        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
        {
            var messageBuilder = MessageBuilder.Create();

            if (type == typeof(Channel))
            {
                var originalChannel = (Channel)original;
                var channel = new CapnpGen.Channel()
                {
                    ChannelId = originalChannel.ChannelId,
                    Name = originalChannel.Name,
                    Subscribers = (uint)originalChannel.Subscribers
                };

                var root = messageBuilder.BuildRoot<CapnpGen.Channel.WRITER>();
                channel.serialize(root);
                var mems = new MemoryStream();
                var pump = new FramePump(mems);
                pump.Send(messageBuilder.Frame);
                byte[] serializedObject = mems.ToArray();
                messageSize = serializedObject.Length;

                return serializedObject;
            }

            if (type == typeof(VideoInfo))
            {
                var originalVideoInfo = (VideoInfo)original;
                var videoInfo = new CapnpGen.VideoInfo()
                {
                    Description = originalVideoInfo.Description,
                    Duration = (ulong)originalVideoInfo.Duration,
                    Qualities = new List<VideoQuality>(originalVideoInfo.Qualities.Select(x => (VideoQuality)x).ToList()),
                    Size = (ulong)originalVideoInfo.Size,
                };
                var root = messageBuilder.BuildRoot<CapnpGen.VideoInfo.WRITER>();
                videoInfo.serialize(root);
                var mems = new MemoryStream();
                var pump = new FramePump(mems);
                pump.Send(messageBuilder.Frame);
                byte[] serializedObject = mems.ToArray();
                messageSize = serializedObject.Length;

                return serializedObject;
            }

            if (type == typeof(SocialInfo))
            {
                var originalSocialInfo = (SocialInfo)original;
                var socialInfo = new CapnpGen.SocialInfo()
                {
                    Comments = originalSocialInfo.Comments.ToArray(),
                    Dislikes = (uint)originalSocialInfo.Dislikes,
                    Likes = (uint)originalSocialInfo.Likes,
                    Views = (uint)originalSocialInfo.Views
                };
                var root = messageBuilder.BuildRoot<CapnpGen.SocialInfo.WRITER>();
                socialInfo.serialize(root);
                var mems = new MemoryStream();
                var pump = new FramePump(mems);
                pump.Send(messageBuilder.Frame);
                byte[] serializedObject = mems.ToArray();
                messageSize = serializedObject.Length;

                return serializedObject;
            }

            if (type == typeof(Video))
            {
                var originalVideo = (Video)original;
                var video = new CapnpGen.Video()
                {
                    Url = originalVideo.Url,
                    VideoId = originalVideo.VideoId,
                    Channel = new CapnpGen.Channel()
                    {
                        ChannelId = originalVideo.Channel.ChannelId,
                        Name = originalVideo.Channel.Name,
                        Subscribers = (uint)originalVideo.Channel.Subscribers
                    },
                    SocialInfo = new CapnpGen.SocialInfo()
                    {
                        Comments = originalVideo.SocialInfo.Comments.ToArray(),
                        Dislikes = (uint)originalVideo.SocialInfo.Dislikes,
                        Likes = (uint)originalVideo.SocialInfo.Likes,
                        Views = (uint)originalVideo.SocialInfo.Views
                    },
                    VideoInfo = new CapnpGen.VideoInfo()
                    {
                        Description = originalVideo.VideoInfo.Description,
                        Duration = (ulong)originalVideo.VideoInfo.Duration,
                        Qualities = new List<VideoQuality>(originalVideo.VideoInfo.Qualities.Select(x => (VideoQuality)x).ToList()),
                        Size = (ulong)originalVideo.VideoInfo.Size,
                    }
                };
                var root = messageBuilder.BuildRoot<CapnpGen.Video.WRITER>();
                video.serialize(root);
                var mems = new MemoryStream();
                var pump = new FramePump(mems);
                pump.Send(messageBuilder.Frame);
                byte[] serializedObject = mems.ToArray();
                messageSize = serializedObject.Length;

                return serializedObject;
            }

            throw new NotImplementedException($"Deserialization for type {type} not implemented!");
        }

        public override Type GetSerializationOutPutType() => typeof(byte[]);

        public override string ToString() => "CapnProto";

        public override bool GetDeserializationResult(Type type, out ISerializationTarget result)
        {
            var intermediateResult = DeserializationResults[type];

            if (type == typeof(Channel))
            {
                var channel = (CapnpGen.Channel)intermediateResult;
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
                var videoInfo = (CapnpGen.VideoInfo)intermediateResult;
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
                var socialInfo = (CapnpGen.SocialInfo)intermediateResult;
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
                var video = (CapnpGen.Video)intermediateResult;
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
    }
}