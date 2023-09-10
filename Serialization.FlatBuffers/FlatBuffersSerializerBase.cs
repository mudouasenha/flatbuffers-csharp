using Google.FlatBuffers;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.FlatBuffers
{
    public abstract class FlatBuffersSerializerBase<ISerializationObject, IModel, IFlatModel> : BaseSerializer<ISerializationObject, IFlatbufferObject>
    {
        internal static FlatBufferBuilder builder = new(1);

        protected override ISerializationObject Serialize<T>(T original, out long messageSize) => Serialize(typeof(T), original, out messageSize);

        public override Type GetSerializationOutPutType() => typeof(byte[]);

        protected override IFlatbufferObject Deserialize<T>(ISerializationObject serializedObject) => Deserialize(typeof(T), serializedObject);

        public static int GetSize()
        {
            // Suggested calculation: buf.Length - buf.Position results in the buffer array size, not the actual size
            // I think offset can be used to get the correct value, since we're clearing the builder every time
            return builder.Offset;
        }

        public override string ToString()
        {
            return "FlatBuffers";
        }
    }
}