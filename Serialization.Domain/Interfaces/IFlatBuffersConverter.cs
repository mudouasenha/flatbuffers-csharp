using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.FlatBuffers.VideoModel;

namespace Serialization.Domain.Interfaces
{
    public interface IFlatBuffersConverter<T, Y> : IConverter<Y> where T : IFlatbufferObject where Y : IFlatBufferSerializable<T, Y>
    {
    }

    public interface IFlatBuffersSocialInfoConverter : IFlatBuffersConverter<SocialInfoFlatModel, SocialInfo>
    {
    }

    public interface IFlatBuffersVideoConverter : IFlatBuffersConverter<VideoFlatModel, Video>
    {
    }

    public interface IFlatBuffersVideoInfoConverter : IFlatBuffersConverter<VideoInfoFlatModel, VideoInfo>
    {
    }

    public interface IFlatBuffersChannelConverter : IFlatBuffersConverter<ChannelFlatModel, Channel>
    {
    }
}