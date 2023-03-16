using FlatBuffers.Domain.Entities;

namespace FlatBuffers.Domain.Services.Abstractions
{
    public interface IVideoService
    {
        Task CreateVideo(VideoEntity video);

        Task DeleteVideo(int videoId);
    }
}