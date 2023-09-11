using FlatBuffersModels;
using Google.FlatBuffers;
using Serialization.Domain.Entities;

namespace Serialization.Serializers.FlatBuffers
{
    public class SocialInfoFlatBuffersSerializer
    {
        private static string[] GetCommentsList(SocialInfoFlatModel flatModel)
        {
            string[] strings = new string[flatModel.CommentsLength];
            for (int i = 0; i < flatModel.CommentsLength; i++)
            {
                strings[i] = flatModel.Comments(i);
            }

            return strings;
        }

        public static SocialInfo FromSerializationModel(SocialInfoFlatModel serialized) =>
            new(serialized.Likes, serialized.Dislikes, GetCommentsList(serialized), serialized.Views);

        public static byte[] Serialize(SocialInfo entity, out long messageSize)
        {
            var builder = new FlatBufferBuilder(1024);
            var stringOffSets = entity.Comments.Select(c => builder.CreateString(c)).ToArray();
            var comments = SocialInfoFlatModel.CreateCommentsVector(builder, stringOffSets);

            SocialInfoFlatModel.StartSocialInfoFlatModel(builder);
            SocialInfoFlatModel.AddLikes(builder, entity.Likes);
            SocialInfoFlatModel.AddDislikes(builder, entity.Dislikes);
            SocialInfoFlatModel.AddComments(builder, comments);
            SocialInfoFlatModel.AddViews(builder, entity.Views);

            var videoInfoOffSet = SocialInfoFlatModel.EndSocialInfoFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);

            messageSize = FlatBuffersSerializerBase.GetSize();

            var byteArray = builder.SizedByteArray();
            builder.Clear();

            return byteArray;
        }
    }
}