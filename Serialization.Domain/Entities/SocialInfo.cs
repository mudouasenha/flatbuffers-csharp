using Google.Protobuf;
using MessagePack;
using ProtoBuf;
using Serialization.Domain.Interfaces;
using Thrift.Protocol;

namespace Serialization.Domain.Entities
{

    [MessagePackObject]
    [Serializable]
    [ProtoContract]
    public class SocialInfo : ISerializationTarget
    {
        [NonSerialized]
        private IMessage<ProtoObjects.SocialInfo> protoObject;

        [NonSerialized]
        private thriftObjects.SocialInfo thriftObject;

        public SocialInfo() { }

        public SocialInfo(int likes, int dislikes, string[] comments, int views)
        {
            Likes = likes;
            Dislikes = dislikes;
            Comments = comments;
            Views = views;
        }

        [Key(0)]
        [ProtoMember(1)]
        public int Likes { get; set; }

        [Key(1)]
        [ProtoMember(2)]
        public int Dislikes { get; set; }

        [Key(2)]
        [ProtoMember(3)]
        public string[] Comments { get; set; }

        [Key(3)]
        [ProtoMember(4)]
        public int Views { get; set; }

        public long Serialize(ISerializer serializer) => serializer.BenchmarkSerialize(this);

        public long Deserialize(ISerializer serializer) => serializer.BenchmarkDeserialize(this);

        public bool Equals(SocialInfo other) => Likes.Equals(other.Likes) && Dislikes.Equals(other.Dislikes) && Comments.Equals(other.Comments) && Views.Equals(other.Views);

        public bool Equals(ISerializationTarget other) => other is SocialInfo otherSocialInfo && Equals(otherSocialInfo);

        public override string ToString()
        {
            return "SocialInfo";
        }

        public void CreateProtobufMessage()
        {
            protoObject = new ProtoObjects.SocialInfo()
            {
                Dislikes = (uint)Dislikes,
                Likes = (uint)Likes,
                Views = (uint)Views,
                Comments = { Comments }
            };
        }

        public IMessage GetProtobufMessage()
        {
            return protoObject;
        }

        public TBase GetThriftMessage()
        {
            return thriftObject;
        }

        public void CreateThriftMessage()
        {
            thriftObject = new thriftObjects.SocialInfo()
            {
                Dislikes = Dislikes,
                Likes = Likes,
                Views = Views,
                Comments = Comments.ToList()
            };
        }
    }
}