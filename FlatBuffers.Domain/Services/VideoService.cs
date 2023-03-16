using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Abstractions;

namespace FlatBuffers.Domain.Services
{
    public class VideoService : IVideoService
    {
        public Task CreateVideo(VideoEntity video)
        {
            throw new NotImplementedException();
        }

        public Task DeleteVideo(int videoId)
        {
            throw new NotImplementedException();
        }
    }
}