using FlatBuffersModels;
using Google.FlatBuffers;
using Serialization.Domain.Entities;

namespace Serialization.Serializers.FlatBuffers
{
    public class VideoInfoFlatBuffersSerializer
    {
        public static VideoInfo FromSerializationModel(VideoInfoFlatModel serialized) => new(serialized.Duration, serialized.Description, serialized.Size, serialized.GetQualitiesArray());

        public static byte[] Serialize(VideoInfo entity, out long messageSize)
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
            messageSize = FlatBuffersSerializer.GetSize();
            var byteArray = builder.SizedByteArray();
            builder.Clear();
            return byteArray;
        }
    }
}