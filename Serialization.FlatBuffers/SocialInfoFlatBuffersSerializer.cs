using FlatBuffersModels;
using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.FlatBuffers
{
    public class SocialInfoFlatBuffersSerializer : FlatBuffersSerializerBase<byte[], SocialInfo, SocialInfoFlatModel>
    {
        protected override IFlatbufferObject Deserialize(Type type, byte[] serializedObject)
        {
            if (type == typeof(SocialInfo))
            {
                var buf = new ByteBuffer(serializedObject);
                var offset = buf.GetInt(buf.Position) + buf.Position;
                var socialInfo = new SocialInfoFlatModel().__assign(offset, buf);
                return socialInfo;
            }
            throw new NotImplementedException($"Deserialization for type {type} not implemented!");
        }

        public override bool GetDeserializationResult(Type type, out ISerializationTarget result)
        {
            if (type == typeof(SocialInfo))
            {
                result = FromSerializationModel((SocialInfoFlatModel)DeserializationResults[typeof(SocialInfoFlatModel)]);
                return true;
            }
            throw new NotImplementedException($"Conversion for type {type} not implemented!");
        }

        public override bool GetSerializationResult(Type type, out object result)
        {
            if (type == typeof(SocialInfo))
            {
                result = SerializationResults[typeof(SocialInfo)].Result;
                return true;
            }
            throw new NotImplementedException($"Conversion for type {type} not implemented!");
        }

        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize) =>
            type == typeof(SocialInfo) ? Serialize((SocialInfo)original, out messageSize) : throw new NotImplementedException($"Serialization for type {type} not implemented!");

        private static string[] GetCommentsList(SocialInfoFlatModel flatModel)
        {
            string[] strings = new string[flatModel.CommentsLength];
            for (int i = 0; i < flatModel.CommentsLength; i++)
            {
                strings[i] = flatModel.Comments(i);
            }

            return strings;
        }

        protected static SocialInfoFlatModel DeserializeFlatModel(ByteBuffer buf) => SocialInfoFlatModel.GetRootAsSocialInfoFlatModel(buf);

        public static SocialInfo FromSerializationModel(SocialInfoFlatModel serialized) =>
            new(serialized.Likes, serialized.Dislikes, GetCommentsList(serialized), serialized.Views);


        protected static SocialInfo GetFromBuffer(ByteBuffer buf) => FromSerializationModel(DeserializeFlatModel(buf));

        private static byte[] Serialize(SocialInfo entity, out long messageSize)
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

            messageSize = GetSize();

            var byteArray = builder.SizedByteArray();
            builder.Clear();

            return byteArray;
        }
    }
}