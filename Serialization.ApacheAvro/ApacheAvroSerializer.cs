using FlatBuffersModels;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;
using SolTechnology.Avro;

namespace Serialization.Serializers.ApacheAvro
{
    public class ApacheAvroSerializer : BaseSerializer<byte[], object>
    {
        protected override object Deserialize<T>(byte[] serializedObject)
        {
            T deserializedObject = AvroConvert.Deserialize<T>(serializedObject);

            return deserializedObject;
        }

        protected override object Deserialize(Type type, byte[] serializedObject)
        {
            if (type == typeof(Channel))
            {
                Channel deserializedObject = AvroConvert.Deserialize(serializedObject, type);
                return deserializedObject;
            }

            if (type == typeof(VideoInfo))
            {
                VideoInfo deserializedObject = AvroConvert.Deserialize(serializedObject, type);
                return deserializedObject;
            }

            if (type == typeof(SocialInfo))
            {
                SocialInfo deserializedObject = AvroConvert.Deserialize(serializedObject, type);
                return deserializedObject;
            }

            if (type == typeof(Video))
            {
                Video deserializedObject = AvroConvert.Deserialize(serializedObject, type);
                return deserializedObject;
            }

            throw new NotImplementedException($"Deserialization for type {type} not implemented!");
        }

        protected override byte[] Serialize<T>(T original, out long messageSize) => Serialize(typeof(T), original, out messageSize);

        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
        {
            byte[] avroObject = AvroConvert.Serialize(original);
            messageSize = avroObject.Length;
            return avroObject;
        }

        public override bool GetDeserializationResult(Type type, out ISerializationTarget result)
        {
            var intermediateResult = DeserializationResults[type];

            if (type == typeof(Channel))
            {
                var channel = (avroObjects.Channel)intermediateResult;
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
                var videoInfo = (avroObjects.VideoInfo)intermediateResult;
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
                var socialInfo = (avroObjects.SocialInfo)intermediateResult;
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
                var video = (avroObjects.Video)intermediateResult;
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

        public override string ToString() => "Avro";

        public override Type GetSerializationOutPutType() => typeof(byte[]);
    }
}