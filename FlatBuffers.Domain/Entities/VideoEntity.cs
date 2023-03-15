using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Receiver.VideoModel;

namespace FlatBuffers.Domain.Entities
{
    public class VideoEntity : IFlatBufferSerializable
    {
        public SocialInfo SocialInfo { get; set; }

        public Channel Channel { get; set; }
    }
}