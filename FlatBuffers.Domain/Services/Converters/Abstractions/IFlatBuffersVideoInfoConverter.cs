using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.VideoModel;

namespace FlatBuffers.Domain.Services.Converters.Abstractions
{
    public interface IFlatBuffersVideoInfoConverter : IFlatBuffersConverter<VideoInfoFlatModel, VideoInfo>
    {
    }
}