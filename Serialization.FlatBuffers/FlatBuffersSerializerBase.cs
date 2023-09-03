using Google.FlatBuffers;
using Serialization.Domain.Interfaces;

namespace Serialization.Serializers.FlatBuffers
{
    public abstract class FlatBuffersSerializerBase<ISerializationObject, Y> : BaseSerializer<ISerializationObject, IFlatbufferObject>
    {
        private readonly FlatBufferBuilder builder;

        public FlatBuffersSerializerBase()
        {
            builder = new FlatBufferBuilder(1);
        }

        //protected abstract ISerializationObject Serialize(Type type, ISerializationObject original, out long messageSize);

        protected override ISerializationObject Serialize<T>(T original, out long messageSize)
        {
            return Serialize(typeof(T), original, out messageSize);
        }

        protected override IFlatbufferObject Deserialize<T>(ISerializationObject serializedObject)
        {
            return Deserialize(typeof(T), serializedObject);
        }

        private int GetSize()
        {
            // Suggested calculation: buf.Length - buf.Position results in the buffer array size, not the actual size
            // I think offset can be used to get the correct value, since we're clearing the builder every time
            return builder.Offset;
        }
    }
}