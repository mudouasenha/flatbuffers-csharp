using FlatBuffersModels;
using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers.SerializationHelpers;

namespace Serialization.Serializers.FlatBuffers
{
    public class FlatBuffersSerializer : BaseSerializer<byte[], IFlatbufferObject>
    {
        internal static FlatBufferBuilder builder = new(1);
        private static string Name = "FlatBuffers";

        protected override byte[] Serialize<T>(T original, out long messageSize) => Serialize(typeof(T), original, out messageSize);

        public override Type GetSerializationOutPutType() => typeof(byte[]);

        protected override IFlatbufferObject Deserialize<T>(byte[] serializedObject) => Deserialize(typeof(T), serializedObject);

        public static int GetSize()
        {
            // Suggested calculation: buf.Length - buf.Position results in the buffer array size, not the actual size
            // I think offset can be used to get the correct value, since we're clearing the builder every time
            return builder.Offset;
        }

        public override bool GetDeserializationResult(Type type, out ISerializationTarget result)
        {
            if (type == typeof(Video))
            {
                result = VideoFlatBuffersSerializer.FromSerializationModel((VideoFlatModel)DeserializationResults[typeof(VideoFlatModel)]);
                return true;
            }
            if (type == typeof(VideoInfo))
            {
                result = VideoInfoFlatBuffersSerializer.FromSerializationModel((VideoInfoFlatModel)DeserializationResults[typeof(VideoInfoFlatModel)]);
                return true;
            }
            if (type == typeof(SocialInfo))
            {
                result = SocialInfoFlatBuffersSerializer.FromSerializationModel((SocialInfoFlatModel)DeserializationResults[typeof(SocialInfoFlatModel)]);
                return true;
            }
            if (type == typeof(Channel))
            {
                result = ChannelFlatBuffersSerializer.FromSerializationModel((ChannelFlatModel)DeserializationResults[typeof(ChannelFlatModel)]);
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
            if (type == typeof(VideoInfo))
            {
                result = SerializationResults[typeof(VideoInfo)].Result;
                return true;
            }
            if (type == typeof(SocialInfo))
            {
                result = SerializationResults[typeof(SocialInfo)].Result;
                return true;
            }
            if (type == typeof(Channel))
            {
                result = SerializationResults[typeof(Channel)].Result;
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
            if (type == typeof(VideoInfo))
            {
                var buf = new ByteBuffer(serializedObject);
                var offset = buf.GetInt(buf.Position) + buf.Position;
                var videoInfo = new VideoInfoFlatModel().__assign(offset, buf);
                return videoInfo;
            }
            if (type == typeof(SocialInfo))
            {
                var buf = new ByteBuffer(serializedObject);
                var offset = buf.GetInt(buf.Position) + buf.Position;
                var socialInfo = new SocialInfoFlatModel().__assign(offset, buf);
                return socialInfo;
            }
            if (type == typeof(Channel))
            {
                var buf = new ByteBuffer(serializedObject);
                var offset = buf.GetInt(buf.Position) + buf.Position;
                var channel = new ChannelFlatModel().__assign(offset, buf);
                return channel;
            }
            throw new NotImplementedException($"Deserialization for type {type} not implemented!");
        }

        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
        {
            switch (type.Name)
            {
                case "Video":
                    return VideoFlatBuffersSerializer.Serialize((Video)original, out messageSize);
                case "SocialInfo":
                    return SocialInfoFlatBuffersSerializer.Serialize((SocialInfo)original, out messageSize);
                case "VideoInfo":
                    return VideoInfoFlatBuffersSerializer.Serialize((VideoInfo)original, out messageSize);
                case "Channel":
                    return ChannelFlatBuffersSerializer.Serialize((Channel)original, out messageSize);
                default:
                    throw new NotImplementedException($"Serialization for type {type} not implemented!");
            }
        }

        public override string ToString()
        {
            return "FlatBuffers";
        }
    }
}