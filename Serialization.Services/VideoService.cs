using Serialization.Domain.Builders;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;

namespace Serialization.Services
{
    public class VideoService : IVideoService
    {
        private readonly VideoBuilder videoBuilder = new();
        public Video CreateVideo() => videoBuilder.Generate();

        public IEnumerable<Video> CreateVideos(int quantity) => videoBuilder.Generate(quantity).ToList();

        public Task DeleteVideo(int videoId)
        {
            throw new NotImplementedException();
        }
    }
}