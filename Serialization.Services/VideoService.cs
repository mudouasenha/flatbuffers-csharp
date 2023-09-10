using Serialization.Domain.Builders;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;

namespace Serialization.Services
{
    public class VideoService
    {
        private readonly VideoBuilder videoBuilder = new();

        public Video CreateVideo() => videoBuilder.Generate();

        public IEnumerable<Video> CreateVideos(int quantity) => videoBuilder.Generate(quantity).ToList();
    }
}