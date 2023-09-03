using Serialization.Domain.FlatBuffers.VideoModel;
using Serialization.Domain.Interfaces;

namespace Serialization.Domain.Entities
{
    public class SocialInfo : IFlatBufferSerializable<SocialInfoFlatModel, SocialInfo>
    {
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Comments { get; set; }
        //public IEnumerable<string> CommentsList { get; set; } = new List<string>();
        public int Views { get; set; }

        public static SocialInfo FromSerializationModel(SocialInfoFlatModel serialized) => new()
        {
            Likes = serialized.Likes,
            Dislikes = serialized.Dislikes,
            Comments = serialized.Comments,
            Views = serialized.Views,
        };

        public long Deserialize(ISerializer serializer)
        {
            return serializer.Deserialize(this);
        }

        public long Serialize(ISerializer serializer)
        {
            return serializer.Serialize(this);
        }
    }
}