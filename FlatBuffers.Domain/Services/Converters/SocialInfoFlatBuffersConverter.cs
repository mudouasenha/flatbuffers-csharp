using BenchmarkDotNet.Attributes;
using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Converters.Abstractions;
using FlatBuffers.Domain.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Services.Converters
{
    public class SocialInfoFlatBuffersConverter : FlatBuffersConverterBase<SocialInfoFlatModel, SocialInfo>, IFlatBuffersSocialInfoConverter
    {
        public override SocialInfo Deserialize(byte[] byteArr)
        {
            var socialInfo = GetFromBuffer(new ByteBuffer(byteArr));

            return socialInfo;
        }

        [Benchmark]
        public override byte[] Serialize(SocialInfo entity)
        {
            var builder = new FlatBufferBuilder(1024);
            SocialInfoFlatModel.StartSocialInfoFlatModel(builder);
            SocialInfoFlatModel.AddLikes(builder, entity.Likes);
            SocialInfoFlatModel.AddDislikes(builder, entity.Dislikes);
            SocialInfoFlatModel.AddComments(builder, entity.Comments);
            SocialInfoFlatModel.AddViews(builder, entity.Views);
            var videoInfoOffSet = SocialInfoFlatModel.EndSocialInfoFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);

            return builder.SizedByteArray();
        }

        [Benchmark]
        protected SocialInfoFlatModel DeserializeFlatModel(ByteBuffer buf) => SocialInfoFlatModel.GetRootAsSocialInfoFlatModel(buf);

        protected override SocialInfo FromSerializationModel(SocialInfoFlatModel serialized) => new()
        {
            Likes = serialized.Likes,
            Dislikes = serialized.Dislikes,
            Comments = serialized.Comments,
            Views = serialized.Views,
        };

        protected override SocialInfo GetFromBuffer(ByteBuffer buf)
        {
            var socialInfo = DeserializeFlatModel(buf);

            return FromSerializationModel(socialInfo);
        }
    }
}