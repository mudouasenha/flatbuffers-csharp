using FlatBuffers.Domain.Entities;

namespace FlatBuffers.Domain.Services.Abstractions
{
    public interface IVideoService
    {
        Video CreateVideo(Video video = null);

        Task DeleteVideo(int videoId);
    }
}