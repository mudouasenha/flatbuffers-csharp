using FlatBuffers.Domain.Entities;

namespace FlatBuffers.Domain.Services.Abstractions
{
    public interface IVideoService
    {
        VideoEntity CreateVideo(VideoEntity video = null);

        Task DeleteVideo(int videoId);
    }
}