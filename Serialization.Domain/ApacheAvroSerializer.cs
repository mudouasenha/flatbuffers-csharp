﻿using Avro.IO;
using Avro.Specific;
using Serialization.Domain.Entities;
using Serialization.Domain.Entities.Enums;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.ApacheAvro
{
    public class ApacheAvroSerializer : BaseSerializer<byte[], ISpecificRecord>
    {
        private readonly string channelSchema = avroObjects.Channel._SCHEMA.ToString();
        private readonly string videoInfoSchema = avroObjects.VideoInfo._SCHEMA.ToString();
        private readonly string socialInfoSchema = avroObjects.SocialInfo._SCHEMA.ToString();
        private readonly string videoSchema = avroObjects.Video._SCHEMA.ToString();

        protected override ISpecificRecord Deserialize<T>(byte[] serializedObject)
        {
            return Deserialize(typeof(T), serializedObject);
        }

        protected override ISpecificRecord Deserialize(Type type, byte[] serializedObject)
        {
            using (MemoryStream inputStream = new MemoryStream(serializedObject))
            {
                if (type == typeof(Channel))
                {
                    SpecificReader<avroObjects.Channel> reader = new SpecificReader<avroObjects.Channel>(avroObjects.Channel._SCHEMA, avroObjects.Channel._SCHEMA);
                    BinaryDecoder decoder = new BinaryDecoder(inputStream);

                    avroObjects.Channel record = reader.Read(null, decoder);

                    return record;
                }

                if (type == typeof(VideoInfo))
                {
                    SpecificReader<avroObjects.VideoInfo> reader = new SpecificReader<avroObjects.VideoInfo>(avroObjects.VideoInfo._SCHEMA, avroObjects.VideoInfo._SCHEMA);
                    BinaryDecoder decoder = new BinaryDecoder(inputStream);

                    avroObjects.VideoInfo record = reader.Read(null, decoder);

                    return record;
                }

                if (type == typeof(SocialInfo))
                {
                    SpecificReader<avroObjects.SocialInfo> reader = new SpecificReader<avroObjects.SocialInfo>(avroObjects.SocialInfo._SCHEMA, avroObjects.SocialInfo._SCHEMA);
                    BinaryDecoder decoder = new BinaryDecoder(inputStream);

                    avroObjects.SocialInfo record = reader.Read(null, decoder);

                    return record;
                }

                if (type == typeof(Video))
                {
                    SpecificReader<avroObjects.Video> reader = new SpecificReader<avroObjects.Video>(avroObjects.Video._SCHEMA, avroObjects.Video._SCHEMA);
                    BinaryDecoder decoder = new BinaryDecoder(inputStream);

                    avroObjects.Video record = reader.Read(null, decoder);

                    return record;
                }
            }

            throw new NotImplementedException($"Deserialization for type {type} not implemented!");
        }

        protected override byte[] Serialize<T>(T original, out long messageSize)
        {
            original.CreateAvroMessage();
            var avroMsg = original.GetAvroMessage();
            avroMsg.GetType();

            using MemoryStream outputStream = new MemoryStream();
            SpecificWriter<ISpecificRecord> writer = new SpecificWriter<ISpecificRecord>(avroMsg.Schema);
            BinaryEncoder encoder = new BinaryEncoder(outputStream);

            writer.Write(avroMsg, encoder);

            encoder.Flush();

            var avroObject = outputStream.ToArray();
            messageSize = avroObject.Length;
            return outputStream.ToArray();
        }

        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
        {
            original.CreateAvroMessage();
            var avroMsg = original.GetAvroMessage();
            avroMsg.GetType();

            using MemoryStream outputStream = new MemoryStream();
            SpecificWriter<ISpecificRecord> writer = new SpecificWriter<ISpecificRecord>(avroMsg.Schema);
            BinaryEncoder encoder = new BinaryEncoder(outputStream);

            writer.Write(avroMsg, encoder);

            encoder.Flush();

            var avroObject = outputStream.ToArray();
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
                    Qualities = (VideoQualities[])videoInfo.Qualities.ToArray().Select(x => (VideoQualities)x)
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
                        Qualities = (VideoQualities[])video.VideoInfo.Qualities.ToArray().Select(x => (VideoQualities)x)
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