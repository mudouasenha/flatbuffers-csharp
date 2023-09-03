using Serialization.Domain.FlatBuffers.VideoModel;
using Serialization.Domain.Interfaces;

namespace Serialization.Domain.Entities
{
    public class SocialInfo : ISerializable
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

        public ISerializable Deserialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ISerializable other)
        {
            throw new NotImplementedException();
        }

        public T Serialize<T>(ISerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}