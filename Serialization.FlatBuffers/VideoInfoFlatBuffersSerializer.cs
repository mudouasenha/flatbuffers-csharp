using FlatBuffersModels;
using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.FlatBuffers
{
    public class VideoInfoFlatBuffersSerializer : FlatBuffersSerializerBase<byte[], VideoInfo, VideoInfoFlatModel>
    {
        protected override IFlatbufferObject Deserialize(Type type, byte[] serializedObject)
        {
            if (type == typeof(VideoInfo))
            {
                var buf = new ByteBuffer(serializedObject);
                var offset = buf.GetInt(buf.Position) + buf.Position;
                var videoInfo = new VideoInfoFlatModel().__assign(offset, buf);
                return videoInfo;
            }
            throw new NotImplementedException($"Deserialization for type {type} not implemented!");
        }

        public override bool GetDeserializationResult(Type type, out ISerializationTarget result)
        {
            if (type == typeof(VideoInfo))
            {
                result = FromSerializationModel((VideoInfoFlatModel)DeserializationResults[typeof(VideoInfoFlatModel)]);
                return true;
            }
            throw new NotImplementedException($"Conversion for type {type} not implemented!");
        }

        public override bool GetSerializationResult(Type type, out object result)
        {
            if (type == typeof(VideoInfo))
            {
                result = SerializationResults[typeof(VideoInfo)].Result;
                return true;
            }
            throw new NotImplementedException($"Conversion for type {type} not implemented!");
        }

        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize) =>
            type == typeof(VideoInfo) ? Serialize((VideoInfo)original, out messageSize) : throw new NotImplementedException($"Serialization for type {type} not implemented!");

        public static VideoInfo FromSerializationModel(VideoInfoFlatModel serialized) => new(serialized.Duration, serialized.Description, serialized.Size, serialized.GetQualitiesArray());

        private static byte[] Serialize(VideoInfo entity, out long messageSize)
        {
            var builder = new FlatBufferBuilder(1024);
            var descriptionOffSet = builder.CreateString(entity.Description);
            var qualitiesVector = VideoInfoFlatModel.CreateQualitiesVector(builder, entity.Qualities);

            VideoInfoFlatModel.StartVideoInfoFlatModel(builder);
            VideoInfoFlatModel.AddDescription(builder, descriptionOffSet);
            VideoInfoFlatModel.AddDuration(builder, entity.Duration);
            VideoInfoFlatModel.AddSize(builder, entity.Size);
            VideoInfoFlatModel.AddQualities(builder, qualitiesVector);

            var videoInfoOffSet = VideoInfoFlatModel.EndVideoInfoFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);
            messageSize = GetSize();
            var byteArray = builder.SizedByteArray();
            builder.Clear();
            return byteArray;
        }
    }
}