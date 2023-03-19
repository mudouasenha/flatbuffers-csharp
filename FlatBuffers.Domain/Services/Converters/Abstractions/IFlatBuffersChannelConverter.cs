using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.VideoModel;

namespace FlatBuffers.Domain.Services.Converters.Abstractions
{
    public interface IFlatBuffersChannelConverter : IFlatBuffersConverter<ChannelFlatModel, Channel>
    {
    }
}