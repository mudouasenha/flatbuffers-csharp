using AutoBogus;
using Bogus;
using Serialization.Domain.Entities;

namespace Serialization.Domain.Builders
{
    public class ChannelBuilder : AutoFaker<Channel>
    {
        public ChannelBuilder()
        {
            var faker = new Faker();
            RuleFor(v => v.Name, f => faker.Random.String2(10, 80));
            RuleFor(v => v.ChannelId, f => Guid.NewGuid().ToString());
            RuleFor(v => v.Subscribers, f => faker.Random.Int(min: 0, max: 1_000_000_000));
        }

        public ChannelBuilder WithChannelId(string channelId)
        {
            RuleFor(x => x.ChannelId, channelId);
            return this;
        }

        public ChannelBuilder WithName(string name)
        {
            RuleFor(x => x.Name, name);
            return this;
        }

        public ChannelBuilder WithSubscribers(int subscribers)
        {
            RuleFor(x => x.Subscribers, subscribers);
            return this;
        }
    }
}
