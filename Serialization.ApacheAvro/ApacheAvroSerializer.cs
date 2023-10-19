using Avro.IO;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using FlatBuffersModels;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.ApacheAvro
{
    public class ApacheAvroSerializer : BaseSerializer<byte[], object>
    {
        private readonly ISchemaRegistryClient schemaRegistryClient;
        private AvroSerializerConfig avroSerializerConfig;

        public ApacheAvroSerializer(ISchemaRegistryClient schemaRegistryClient, AvroSerializerConfig avroSerializerConfig = null)
        {
            this.schemaRegistryClient = schemaRegistryClient;
            this.avroSerializerConfig = avroSerializerConfig ?? new AvroSerializerConfig();
        }

        #region Serialization

        protected override byte[] Serialize<T>(T original, out long messageSize) => Serialize(typeof(T), original, out messageSize);

        public override Type GetSerializationOutPutType() => typeof(byte[]);

        //protected override TBase Deserialize<T>(byte[] serializedObject) => Deserialize(typeof(T), serializedObject);


        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
        {
            var obj = new avroObjects.Channel();
            using (MemoryStream stream = new MemoryStream())
            {
                var config = new AvroSerializerConfig { AutoRegisterSchemas = true, a = MyAvroRecord.SCHEMA$.ToString() };
                var src = new CachedSchemaRegistryClient(config);
                var avroSerializer = new AvroSerializer<avroObjects.Channel>(avroObjects.Channel, config);
                var encoder = new BinaryEncoder(stream);
                avroSerializer.SerializeAsync(obj, original);
                messageSize = stream.Length;
                return stream.ToArray();

            }
        }

        #endregion

        #region Deserialization

        protected override TBase Deserialize<T>(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                var avroDeserializer = new AvroSerializer<T>();
                var decoder = new BinaryDecoder(stream);
                return avroDeserializer.Deserialize(decoder);
            }
        }

        protected override object Deserialize(Type type, byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                var avroDeserializer = AvroSerializer.Create(type);
                var decoder = new BinaryDecoder(stream);
                return avroDeserializer.Deserialize(decoder);
            }
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

        #endregion

        public override string ToString()
        {
            return "Avro";
        }

        public override bool GetSerializationResult(Type type, out object result)
        {
            throw new NotImplementedException();
        }
    }
}
