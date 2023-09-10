using AutoBogus;
using Serialization.Domain.Entities;

namespace Serialization.Domain.Builders
{
    public class VideoBuilder : AutoFaker<Video>
    {
        public VideoBuilder()
        {
            RuleFor(v => v.VideoInfo, f => new VideoInfoBuilder().Generate());
            RuleFor(v => v.Channel, f => new ChannelBuilder().Generate());
            RuleFor(v => v.SocialInfo, f => new SocialInfoBuilder().Generate());
        }

        public VideoBuilder WithVideoInfo(VideoInfo videoInfo)
        {
            RuleFor(x => x.VideoInfo, videoInfo);
            return this;
        }

        public VideoBuilder WithChannel(Channel channel)
        {
            RuleFor(x => x.Channel, channel);
            return this;
        }

        public VideoBuilder WithSocialInfo(SocialInfo socialInfo)
        {
            RuleFor(x => x.SocialInfo, socialInfo);
            return this;
        }
    }
}
