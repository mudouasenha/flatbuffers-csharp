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
                result = VideoFlatBuffersSerializer.FromSerializationModel((VideoFlatModel)DeserializationResults[typeof(Video)]);
                return true;
            }
            if (type == typeof(VideoInfo))
            {
                result = VideoInfoFlatBuffersSerializer.FromSerializationModel((VideoInfoFlatModel)DeserializationResults[typeof(VideoInfo)]);
                return true;
            }
            if (type == typeof(SocialInfo))
            {
                result = SocialInfoFlatBuffersSerializer.FromSerializationModel((SocialInfoFlatModel)DeserializationResults[typeof(SocialInfo)]);
                return true;
            }
            if (type == typeof(Channel))
            {
                result = ChannelFlatBuffersSerializer.FromSerializationModel((ChannelFlatModel)DeserializationResults[typeof(Channel)]);
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
            return type.Name switch
            {
                "Video" => VideoFlatBuffersSerializer.Serialize((Video)original, out messageSize),
                "SocialInfo" => SocialInfoFlatBuffersSerializer.Serialize((SocialInfo)original, out messageSize),
                "VideoInfo" => VideoInfoFlatBuffersSerializer.Serialize((VideoInfo)original, out messageSize),
                "Channel" => ChannelFlatBuffersSerializer.Serialize((Channel)original, out messageSize),
                _ => throw new NotImplementedException($"Serialization for type {type} not implemented!"),
            };
        }

        public override string ToString()
        {
            return "FlatBuffers";
        }
    }
}