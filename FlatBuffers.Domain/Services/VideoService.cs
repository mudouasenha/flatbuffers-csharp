using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Abstractions;

namespace FlatBuffers.Domain.Services
{
    public class VideoService : IVideoService
    {
        public VideoEntity CreateVideo(VideoEntity video = null)
        {
            if (video == null)
                return new VideoEntity()
                {
                    ChannelEntity = new ChannelEntity()
                    {
                        ChannelId = 1,
                        Name = "Matheus's Channel",
                        Subscribers = 5000
                    },
                    SocialInfoEntity = new SocialInfoEntity()
                    {
                        Comments = 10,
                        Dislikes = 1,
                        Likes = 10000,
                        Views = 1_000_000
                    },
                    VideoInfoEntity = new VideoInfoEntity()
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