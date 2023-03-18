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
            throw new NotImplementedException();
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
            var socialInfo = SocialInfoFlatModel.GetRootAsSocialInfo(buf);

            return FromSerializationModel(socialInfo);
        }
    }
}