using FlatBuffers.Domain.Interfaces;
using FlatBuffers.Receiver.VideoModel;
using Google.FlatBuffers;

namespace FlatBuffers.Domain.Entities
{
    public class SocialInfoEntity : IFlatBufferSerializable<SocialInfo, SocialInfoEntity>
    {
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Comments { get; set; }
        public int Views { get; set; }

        public ByteBuffer CreateBuffer(FlatBufferBuilder builder, SocialInfoEntity entity)
        {
            throw new NotImplementedException();
        }

        public SocialInfoEntity FromSerializationModel(SocialInfo serialized) => new()
        {
            Likes = serialized.Likes,
            Dislikes = serialized.Dislikes,
            Comments = serialized.Comments,
            Views = serialized.Views,
        };

        public SocialInfoEntity GetFromBuffer(ByteBuffer buf)
        {
            var socialInfo = SocialInfo.GetRootAsSocialInfo(buf);

            return FromSerializationModel(socialInfo);
        }
    }
}