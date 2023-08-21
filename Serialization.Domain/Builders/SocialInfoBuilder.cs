using AutoBogus;
using Bogus;
using Serialization.Domain.Entities;

namespace Serialization.Domain.Builders
{
    public class SocialInfoBuilder : AutoFaker<SocialInfo>
    {
        public SocialInfoBuilder()
        {
            var faker = new Faker();
            RuleFor(v => v.Dislikes, f => faker.Random.Int());
            RuleFor(v => v.Likes, f => faker.Random.Int());
            RuleFor(v => v.Views, f => faker.Random.Int());
            RuleFor(v => v.Comments, f => faker.Random.Int());
        }


        public SocialInfoBuilder WithDislikes(int dislikes)
        {
            RuleFor(x => x.Dislikes, dislikes);
            return this;
        }

        public SocialInfoBuilder WithLikes(int likes)
        {
            RuleFor(x => x.Likes, likes);
            return this;
        }

        public SocialInfoBuilder WithViews(int views)
        {
            RuleFor(x => x.Views, views);
            return this;
        }

        public SocialInfoBuilder WithComments(int comments)
        {
            RuleFor(x => x.Comments, comments);
            return this;
        }
    }
}
