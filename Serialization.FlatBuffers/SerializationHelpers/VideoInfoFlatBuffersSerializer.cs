using FlatBuffersModels;
using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.Entities.Enums;

namespace Serialization.Serializers.FlatBuffers.SerializationHelpers
{
    public class VideoInfoFlatBuffersSerializer
    {
        public static VideoInfo FromSerializationModel(VideoInfoFlatModel serialized) => new(serialized.Duration, serialized.Description, serialized.Size, serialized.GetQualitiesArray().Select(x => (VideoQualities)x).ToArray());

        public static byte[] Serialize(VideoInfo entity, out long messageSize)
        {
            var builder = new FlatBufferBuilder(1024);
            var descriptionOffSet = builder.CreateString(entity.Description);
            var qualities = entity.Qualities.Select(x => (VideoQualityFlatModel)x).ToArray();
            var qualitiesVector = VideoInfoFlatModel.CreateQualitiesVector(builder, qualities);

            VideoInfoFlatModel.StartVideoInfoFlatModel(builder);
            VideoInfoFlatModel.AddDescription(builder, descriptionOffSet);
            VideoInfoFlatModel.AddDuration(builder, entity.Duration);
            VideoInfoFlatModel.AddSize(builder, entity.Size);
            VideoInfoFlatModel.AddQualities(builder, qualitiesVector);

            var videoInfoOffSet = VideoInfoFlatModel.EndVideoInfoFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);
            messageSize = builder.Offset;
            var byteArray = builder.SizedByteArray();
            builder.Clear();
            return byteArray;
        }
    }
}