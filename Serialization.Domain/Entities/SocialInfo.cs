using MessagePack;
using Serialization.Domain.Interfaces;

namespace Serialization.Domain.Entities
{

    [MessagePackObject]
    [Serializable]
    public class SocialInfo : ISerializationTarget
    {
        public SocialInfo() { }

        public SocialInfo(int likes, int dislikes, string[] comments, int views)
        {
            Likes = likes;
            Dislikes = dislikes;
            Comments = comments;
            Views = views;
        }

        [Key(0)]
        public int Likes { get; set; }

        [Key(1)]
        public int Dislikes { get; set; }

        [Key(2)]
        public string[] Comments { get; set; }

        [Key(3)]
        public int Views { get; set; }

        public long Serialize(ISerializer serializer) => serializer.BenchmarkSerialize(this);

        public long Deserialize(ISerializer serializer) => serializer.BenchmarkDeserialize(this);

        public bool Equals(SocialInfo other) => Likes.Equals(other.Likes) && Dislikes.Equals(other.Dislikes) && Comments.Equals(other.Comments) && Views.Equals(other.Views);

        public bool Equals(ISerializationTarget other) => other is SocialInfo otherSocialInfo && Equals(otherSocialInfo);

        public override string ToString()
        {
            return "SocialInfo";
        }

        public long Serialize(ref byte[] target)
        {
            throw new NotImplementedException();
        }

        public long Deserialize(ref byte[] target)
        {
            throw new NotImplementedException();
        }
    }
}