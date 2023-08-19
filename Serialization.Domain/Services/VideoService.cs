using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Abstractions;

namespace FlatBuffers.Domain.Services
{
    public class VideoService : IVideoService
    {
        public Video CreateVideo(Video video = null)
        {
            if (video == null)
                return new Video()
                {
                    Channel = new Channel()
                    {
                        ChannelId = 1,
                        Name = "Matheus's Channel",
                        Subscribers = 5000
                    },
                    SocialInfo = new SocialInfo()
                    {
                        Comments = 10,
                        Dislikes = 1,
                        Likes = 10000,
                        Views = 1_000_000
                    },
                    VideoInfo = new VideoInfo()
                    {
                        Description = "Description test",
                        Duration = 3600,
                        Size = 1_000_000
                    }
                };

            return video;
        }

        public Task DeleteVideo(int videoId)
        {
            throw new NotImplementedException();
        }
    }
}