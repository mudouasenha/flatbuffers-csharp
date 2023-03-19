using BenchmarkDotNet.Attributes;
using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Converters.Abstractions;
using FlatBuffers.Domain.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Services.Converters
{
    public class SocialInfoFlatBuffersConverter : FlatBuffersConverterBase<SocialInfoFlatModel, SocialInfo>, IFlatBuffersSocialInfoConverter
    {
        private readonly FlatBufferBuilder _flatBufferBuilder;

        public SocialInfoFlatBuffersConverter() => _flatBufferBuilder = new FlatBufferBuilder(1024);

        public override SocialInfo Deserialize(byte[] byteArr)
        {
            var socialInfo = GetFromBuffer(new ByteBuffer(byteArr));

            return socialInfo;
        }

        public override byte[] Serialize(SocialInfo socialInfo)
        {
            var buf = CreateBuffer(_flatBufferBuilder, socialInfo);

            return buf.ToFullArray();
        }

        [Benchmark]
        protected override ByteBuffer CreateBuffer(FlatBufferBuilder builder, SocialInfo entity)
        {
            SocialInfoFlatModel.StartSocialInfoFlatModel(builder);
            SocialInfoFlatModel.AddLikes(builder, entity.Likes);
            SocialInfoFlatModel.AddDislikes(builder, entity.Dislikes);
            SocialInfoFlatModel.AddComments(builder, entity.Comments);
            SocialInfoFlatModel.AddViews(builder, entity.Views);
            var videoInfoOffSet = SocialInfoFlatModel.EndSocialInfoFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);

            return builder.DataBuffer;
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