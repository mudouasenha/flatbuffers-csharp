using Serialization.Domain.Entities;

namespace Serialization.Domain.Interfaces
{
    public interface IVideoService
    {
        Video CreateVideo();

        IEnumerable<Video> CreateVideos(int quantity);

        Task DeleteVideo(int videoId);
    }
}