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
            RuleFor(v => v.Dislikes, f => faker.Random.Int(min: 0, max: 1_000_000_000));
            RuleFor(v => v.Likes, f => faker.Random.Int(min: 0, max: 1_000_000_000));
            RuleFor(v => v.Views, f => faker.Random.Int(min: 0, max: 1_000_000_000));
            RuleFor(v => v.Comments, f => faker.Make(faker.Random.Number(1, 10), () => faker.Random.String2(5, 300)).ToArray());
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

        public SocialInfoBuilder WithComments(string[] comments)
        {
            RuleFor(x => x.Comments, comments);
            return this;
        }

        public SocialInfoBuilder WithSeveralComments(int min, int max)
        {
            var faker = new Faker();
            var randomNum = new Faker();
            RuleFor(v => v.Comments, f => faker.Make(faker.Random.Number(min, max), () => faker.Lorem.Paragraph(5)).ToArray());
            return this;
        }
    }
}
