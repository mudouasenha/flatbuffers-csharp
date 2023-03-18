using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Domain.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Entities
{
    public class SocialInfo : IFlatBufferSerializable<SocialInfoFlatModel, SocialInfo>
    {
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Comments { get; set; }
        public int Views { get; set; }

        public ByteBuffer CreateBuffer(FlatBufferBuilder builder, SocialInfo entity)
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

        public SocialInfo FromSerializationModel(SocialInfoFlatModel serialized) => new()
        {
            Likes = serialized.Likes,
            Dislikes = serialized.Dislikes,
            Comments = serialized.Comments,
            Views = serialized.Views,
        };

        public SocialInfo GetFromBuffer(ByteBuffer buf)
        {
            var socialInfo = SocialInfoFlatModel.GetRootAsSocialInfoFlatModel(buf);

            return FromSerializationModel(socialInfo);
        }
    }
}