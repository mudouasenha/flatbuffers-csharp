using Serialization.Domain.Interfaces;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serialiazation.Serializers.Manual
{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable SYSLIB0011 // Type or member is obsolete

    public class BinaryFormatterSerializer : BaseDirectSerializer<byte[]>
    {
        #region Serialization

        protected override byte[] Serialize<T>(T original, out long messageSize)
        {
            byte[] serializedData;
            BinaryFormatter formatter = new();

            using (MemoryStream memoryStream = new())
            {
                formatter.Serialize(memoryStream, original);
                serializedData = memoryStream.ToArray();
            }

            messageSize = serializedData.Length;
            return serializedData;
        }

        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
        {
            byte[] serializedData;
            BinaryFormatter formatter = new();

            using (MemoryStream memoryStream = new())
            {
                formatter.Serialize(memoryStream, original);
                serializedData = memoryStream.ToArray();
            }

            messageSize = serializedData.Length;
            return serializedData;
        }

        #endregion Serialization

        #region Deserialization

        protected override ISerializationTarget Deserialize<T>(byte[] bytes)
        {
            BinaryFormatter formatter = new();

            ISerializationTarget instance = Activator.CreateInstance<T>();

            using (MemoryStream memoryStream = new(bytes))
            {
                instance = (ISerializationTarget)formatter.Deserialize(memoryStream);
            }

            return instance;
        }

        protected override ISerializationTarget Deserialize(Type type, byte[] bytes)
        {
            BinaryFormatter formatter = new();

            ISerializationTarget instance = (ISerializationTarget)Activator.CreateInstance(type);

            using (MemoryStream memoryStream = new(bytes))
            {
                instance = (ISerializationTarget)formatter.Deserialize(memoryStream);
            }

            return instance;
        }

        #endregion Deserialization

        public override string ToString() => "BinaryFormatter";

        public override Type GetSerializationOutPutType() => typeof(byte[]);
    }
}