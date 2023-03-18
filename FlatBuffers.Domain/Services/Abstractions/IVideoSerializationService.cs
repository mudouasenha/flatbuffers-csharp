using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.VideoModel;

namespace FlatBuffers.Domain.Services.Abstractions
{
    public interface IVideoSerializationService : ISerializationService<VideoFlatModel, Video>
    {
    }
}