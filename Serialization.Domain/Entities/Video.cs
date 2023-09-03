using Serialization.Domain.FlatBuffers.VideoModel;
using Serialization.Domain.Interfaces;

namespace Serialization.Domain.Entities
{
    public class Video : ISerializable
    {
        public SocialInfo SocialInfo { get; set; }

        public Channel Channel { get; set; }

        public VideoInfo VideoInfo { get; set; }

        public static Video FromSerializationModel(VideoFlatModel video) => new()
        {
            SocialInfo = SocialInfo.FromSerializationModel(video.SocialInfo.Value),
            Channel = Channel.FromSerializationModel(video.Channel.Value),
            VideoInfo = VideoInfo.FromSerializationModel(video.VideoInfo.Value)
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