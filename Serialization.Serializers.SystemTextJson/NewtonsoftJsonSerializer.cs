using Newtonsoft.Json;
using Serialization.Domain.Interfaces;
using System.Text;

namespace Serialization.Serializers.SystemTextJson
{
    public class NewtonsoftJsonSerializer : BaseDirectSerializer<byte[]>
    {
        private readonly JsonSerializer jsonSerializer;

        public NewtonsoftJsonSerializer()
        {
            jsonSerializer = new JsonSerializer();
        }

        #region Serialization

        protected override byte[] Serialize<T>(T original, out long messageSize)
        {
            var json = JsonConvert.SerializeObject(original);
            byte[] data = Encoding.UTF8.GetBytes(json);
            messageSize = data.Length;
            return data;
        }

        protected override byte[] Serialize(Type type, ISerializationTarget original, out long messageSize)
        {
            var json = JsonConvert.SerializeObject(original);
            byte[] data = Encoding.UTF8.GetBytes(json);
            messageSize = data.Length;
            return data;
        }

        #endregion Serialization

        #region Deserialization

        protected override ISerializationTarget Deserialize<T>(byte[] data)
        {
            string json = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected override ISerializationTarget Deserialize(Type type, byte[] data)
        {
            string json = Encoding.UTF8.GetString(data);
            return (ISerializationTarget)JsonConvert.DeserializeObject(json, type);
        }

        #endregion Deserialization

        public override string ToString()
        {
            return "Newtonsoft.Json";
        }

        public override Type GetSerializationOutPutType() => typeof(MemoryStream);
    }
}