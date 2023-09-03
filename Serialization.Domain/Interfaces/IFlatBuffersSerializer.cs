using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.FlatBuffers.VideoModel;

namespace Serialization.Domain.Interfaces
{
    public interface IFlatBuffersSerializer<T, Y> : ISerializer<Y> where T : IFlatbufferObject where Y : IFlatBufferSerializable<T, Y>
    {
    }

    public interface IFlatBuffersSocialInfoSerializer : IFlatBuffersSerializer<SocialInfoFlatModel, SocialInfo>
    {
    }

    public interface IFlatBuffersVideoSerializer : IFlatBuffersSerializer<VideoFlatModel, Video>
    {
    }

    public interface IFlatBuffersVideoInfoSerializer : IFlatBuffersSerializer<VideoInfoFlatModel, VideoInfo>
    {
    }

    public interface IFlatBuffersChannelSerializer : IFlatBuffersSerializer<ChannelFlatModel, Channel>
    {
    }
}