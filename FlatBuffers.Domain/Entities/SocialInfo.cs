using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Domain.VideoModel;

namespace FlatBuffers.Domain.Entities
{
    public class SocialInfo : IFlatBufferSerializable<SocialInfoFlatModel, SocialInfo>
    {
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Comments { get; set; }
        public int Views { get; set; }

        public static SocialInfo FromSerializationModel(SocialInfoFlatModel serialized) => new()
        {
            Likes = serialized.Likes,
            Dislikes = serialized.Dislikes,
            Comments = serialized.Comments,
            Views = serialized.Views,
        };
    }
}