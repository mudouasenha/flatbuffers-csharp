using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.FlatBuffers.VideoModel;

namespace Serialization.Serializers.FlatBuffers
{
    public class VideoInfoFlatBuffersSerializer : FlatBuffersSerializerBase<byte[], VideoInfoFlatModel>
    {
        public VideoInfo Deserialize(byte[] byteArr)
        {
            var videoInfo = GetFromBuffer(new ByteBuffer(byteArr));

            return videoInfo;
        }

        public byte[] Serialize(VideoInfo entity)
        {
            var builder = new FlatBufferBuilder(1024);

            VideoInfoFlatModel.StartVideoInfoFlatModel(builder);
            VideoInfoFlatModel.AddDescription(builder, builder.CreateString(entity.Description));
            VideoInfoFlatModel.AddDuration(builder, entity.Duration);
            VideoInfoFlatModel.AddSize(builder, entity.Size);
            VideoInfoFlatModel.AddQualities(builder, VideoInfoFlatModel.CreateQualitiesVector(builder, entity.Qualities));
            var videoInfoOffSet = VideoInfoFlatModel.EndVideoInfoFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);

            return builder.SizedByteArray();
        }

        protected override IFlatbufferObject Deserialize(Type type, byte[] serializedObject)
        {
            throw new NotImplementedException();
        }

        protected VideoInfoFlatModel DeserializeFlatModel(ByteBuffer buf) => VideoInfoFlatModel.GetRootAsVideoInfoFlatModel(buf);

        protected VideoInfo FromSerializationModel(VideoInfoFlatModel serialized) => new()
        {
            Duration = serialized.Duration,
            Description = serialized.Description,
            Size = serialized.Size,
            Qualities = serialized.GetQualitiesArray()
        };

        protected VideoInfo GetFromBuffer(ByteBuffer buf)
        {
            var videoInfo = DeserializeFlatModel(buf);

            return FromSerializationModel(videoInfo);
        }

        protected override byte[] Serialize(Type type, byte[] original, out long messageSize)
        {
            throw new NotImplementedException();
        }
    }
}