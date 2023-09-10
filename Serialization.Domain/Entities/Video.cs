using MessagePack;
using Serialization.Domain.Interfaces;

namespace Serialization.Domain.Entities
{

    [MessagePackObject]
    public class Video : ISerializationTarget
    {
        public Video(SocialInfo socialInfo, Channel channel, VideoInfo videoInfo)
        {
            SocialInfo = socialInfo;
            Channel = channel;
            VideoInfo = videoInfo;
        }

        [Key(0)]
        public SocialInfo SocialInfo { get; set; }

        [Key(1)]
        public Channel Channel { get; set; }

        [Key(2)]
        public VideoInfo VideoInfo { get; set; }

        public long Serialize(ISerializer serializer) => serializer.BenchmarkSerialize(this);

        public long Deserialize(ISerializer serializer) => serializer.BenchmarkDeserialize(this);

        public bool Equals(Video other) => SocialInfo.Equals(other.SocialInfo) && Channel.Equals(other.Channel) && VideoInfo.Equals(other.VideoInfo);

        public bool Equals(ISerializationTarget other) => other is Video otherVideo && Equals(otherVideo);

        public override string ToString()
        {
            return "Video";
        }
    }
}