using FlatBuffers.Domain.Entities;
using FlatBuffers.Receiver.VideoModel;

namespace FlatBuffers.Domain.Services.Abstractions
{
    public interface IVideoSerializationService : ISerializationService<Video, VideoEntity>
    {
    }
}