using Serialization.Domain.Interfaces;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serialiazation.Serializers.Manual
{
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

        #endregion

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

        #endregion

        public override string ToString() => "BinaryFormatter";

        public override Type GetSerializationOutPutType() => typeof(byte[]);
    }
}