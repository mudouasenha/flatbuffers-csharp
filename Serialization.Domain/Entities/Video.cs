using Google.Protobuf;
using Google.Protobuf.Collections;
using MessagePack;
using ProtoBuf;
using Serialization.Domain.Interfaces;

namespace Serialization.Domain.Entities
{

    [MessagePackObject]
    [Serializable]
    [ProtoContract]
    public class Video : ISerializationTarget
    {
        [NonSerialized]
        private IMessage<ProtoObjects.Video> protoObject;

        public Video() { }

        public Video(string videoId, string url, SocialInfo socialInfo, Channel channel, VideoInfo videoInfo)
        {
            VideoId = videoId;
            Url = url;
            SocialInfo = socialInfo;
            Channel = channel;
            VideoInfo = videoInfo;
        }

        [Key(0)]
        [ProtoMember(1)]
        public string VideoId { get; set; }

        [Key(1)]
        [ProtoMember(2)]
        public string Url { get; set; }

        [Key(2)]
        [ProtoMember(3)]
        public SocialInfo SocialInfo { get; set; }

        [Key(3)]
        [ProtoMember(4)]
        public Channel Channel { get; set; }

        [Key(4)]
        [ProtoMember(5)]
        public VideoInfo VideoInfo { get; set; }

        public long Serialize(ISerializer serializer) => serializer.BenchmarkSerialize(this);

        public long Deserialize(ISerializer serializer) => serializer.BenchmarkDeserialize(this);

        public bool Equals(Video other) => SocialInfo.Equals(other.SocialInfo) && Channel.Equals(other.Channel) && VideoInfo.Equals(other.VideoInfo);

        public bool Equals(ISerializationTarget other) => other is Video otherVideo && Equals(otherVideo);

        public override string ToString()
        {
            return "Video";
        }

        public void CreateProtobufMessage()
        {
            var qualities = new RepeatedField<ProtoObjects.VideoInfo.Types.VideoQualities>();
            qualities.AddRange(VideoInfo.Qualities.ToArray().Select(x => (ProtoObjects.VideoInfo.Types.VideoQualities)x));


            protoObject = new ProtoObjects.Video()
            {
                VideoId = VideoId,
                Url = Url,
                Channel = new ProtoObjects.Channel()
                {
                    ChannelId = Channel.ChannelId,
                    Name = Channel.Name,
                    Subscribers = (uint)Channel.Subscribers
                },
                SocialInfo = new ProtoObjects.SocialInfo()
                {
                    Dislikes = (uint)SocialInfo.Dislikes,
                    Likes = (uint)SocialInfo.Likes,
                    Views = (uint)SocialInfo.Views,
                    Comments = { SocialInfo.Comments }
                },
                VideoInfo = new ProtoObjects.VideoInfo()
                {
                    Duration = (ulong)VideoInfo.Duration,
                    Description = VideoInfo.Description,
                    Size = (ulong)VideoInfo.Size,
                    Qualities = { qualities }
                },
            };
        }

        public IMessage GetProtobufMessage()
        {
            return protoObject;
        }
    }
}