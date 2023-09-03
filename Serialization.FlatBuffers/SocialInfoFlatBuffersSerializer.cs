using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.FlatBuffers.VideoModel;

namespace Serialization.Serializers.FlatBuffers
{
    public class SocialInfoFlatBuffersSerializer : FlatBuffersSerializerBase<byte[], SocialInfoFlatModel>
    {
        public SocialInfo Deserialize(byte[] byteArr)
        {
            var socialInfo = GetFromBuffer(new ByteBuffer(byteArr));

            return socialInfo;
        }

        public byte[] Serialize(SocialInfo entity)
        {
            var builder = new FlatBufferBuilder(1024);
            SocialInfoFlatModel.StartSocialInfoFlatModel(builder);
            SocialInfoFlatModel.AddLikes(builder, entity.Likes);
            SocialInfoFlatModel.AddDislikes(builder, entity.Dislikes);
            SocialInfoFlatModel.AddComments(builder, entity.Comments);
            SocialInfoFlatModel.AddViews(builder, entity.Views);
            var videoInfoOffSet = SocialInfoFlatModel.EndSocialInfoFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);

            return builder.SizedByteArray();
        }

        protected override IFlatbufferObject Deserialize(Type type, byte[] serializedObject)
        {
            throw new NotImplementedException();
        }

        protected SocialInfoFlatModel DeserializeFlatModel(ByteBuffer buf) => SocialInfoFlatModel.GetRootAsSocialInfoFlatModel(buf);

        protected SocialInfo FromSerializationModel(SocialInfoFlatModel serialized) => new()
        {
            Likes = serialized.Likes,
            Dislikes = serialized.Dislikes,
            Comments = serialized.Comments,
            Views = serialized.Views,
        };

        protected SocialInfo GetFromBuffer(ByteBuffer buf)
        {
            var socialInfo = DeserializeFlatModel(buf);

            return FromSerializationModel(socialInfo);
        }

        protected override byte[] Serialize(Type type, byte[] original, out long messageSize)
        {
            throw new NotImplementedException();
        }
    }
}