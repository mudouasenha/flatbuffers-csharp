using FlatBuffersModels;
using Google.FlatBuffers;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.FlatBuffers
{
    public class ChannelFlatBuffersSerializer : FlatBuffersSerializerBase<byte[], Channel, ChannelFlatModel>
    {
        protected override IFlatbufferObject Deserialize(Type type, byte[] serializedObject)
        {
            if (type == typeof(Channel))
            {
                var buf = new ByteBuffer(serializedObject);
                var offset = buf.GetInt(buf.Position) + buf.Position;
                var channel = new ChannelFlatModel().__assign(offset, buf);
                return channel;
            }
            throw new NotImplementedException($"Deserialization for type {type} not implemented!");
        }

        public override bool GetDeserializationResult(Type type, out ISerializationTarget result)
        {
            if (type == typeof(Channel))
            {
                result = FromSerializationModel((ChannelFlatModel)DeserializationResults[typeof(ChannelFlatModel)]);
                return true;
            }
            throw new NotImplementedException($"Conversion for type {type} not implemented!");
        }

        public override bool GetSerializationResult(Type type, out object result)
        {
            if (type == typeof(Channel))
            {
                result = SerializationResults[typeof(Channel)].Result;
                return true;
            }
            throw new NotImplementedException($"Conversion for type {type} not implemented!");
        }


        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize) =>
            type == typeof(Channel) ? Serialize((Channel)original, out messageSize) : throw new NotImplementedException($"Serialization for type {type} not implemented!");

        protected static ChannelFlatModel DeserializeFlatModel(ByteBuffer buf) => ChannelFlatModel.GetRootAsChannelFlatModel(buf);

        public static Channel FromSerializationModel(ChannelFlatModel serialized) => new(serialized.Name, serialized.Subscribers, serialized.ChannelId);

        private static byte[] Serialize(Channel entity, out long messageSize)
        {
            var builder = new FlatBufferBuilder(1024);
            var name = builder.CreateString(entity.Name);

            ChannelFlatModel.StartChannelFlatModel(builder);
            ChannelFlatModel.AddName(builder, name);
            ChannelFlatModel.AddSubscribers(builder, entity.Subscribers);
            ChannelFlatModel.AddChannelId(builder, entity.ChannelId);

            var videoInfoOffSet = ChannelFlatModel.EndChannelFlatModel(builder);

            builder.Finish(videoInfoOffSet.Value);

            messageSize = GetSize();

            var byteArray = builder.SizedByteArray();
            builder.Clear();
            return byteArray;
        }
    }
}