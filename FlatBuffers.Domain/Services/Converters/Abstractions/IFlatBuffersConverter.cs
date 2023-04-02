using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Domain.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Services.Converters.Abstractions
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