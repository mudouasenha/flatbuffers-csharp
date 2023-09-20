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
            RuleFor(v => v.Name, f => faker.Person.FullName);
            RuleFor(v => v.ChannelId, f => faker.UniqueIndex);
            RuleFor(v => v.Subscribers, f => faker.Random.Int(0));
        }

        public ChannelBuilder WithChannelId(int channelId)
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
