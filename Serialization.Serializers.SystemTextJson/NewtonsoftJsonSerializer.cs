using Newtonsoft.Json;
using Serialization.Domain.Interfaces;
using System.Text;

namespace Serialization.Serializers.SystemTextJson
{
    public class NewtonsoftJsonSerializer : BaseDirectSerializer<MemoryStream>
    {
        private readonly JsonSerializer jsonSerializer;

        public NewtonsoftJsonSerializer()
        {
            jsonSerializer = new JsonSerializer();
        }

        #region Serialization

        protected override MemoryStream Serialize<T>(T original, out long messageSize)
        {
            var stream = new MemoryStream();
            using var tw = new StreamWriter(stream, Encoding.UTF8, 4096, true);
            using var jw = new JsonTextWriter(tw);
            jsonSerializer.Serialize(jw, original);

            // Flush needed for moving the stream position
            jw.Flush();

            messageSize = stream.Position;
            return stream;
        }

        protected override MemoryStream Serialize(Type type, ISerializationTarget original, out long messageSize)
        {
            var stream = new MemoryStream();
            using var tw = new StreamWriter(stream, Encoding.UTF8, 4096, true);
            using var jw = new JsonTextWriter(tw);
            jsonSerializer.Serialize(jw, original, type);

            // Flush needed for moving the stream position
            jw.Flush();

            messageSize = stream.Position;
            return stream;
        }

        #endregion

        #region Deserialization

        protected override ISerializationTarget Deserialize<T>(MemoryStream stream)
        {
            T copy;
            stream.Position = 0;
            using var tr = new StreamReader(stream, Encoding.UTF8, false, 4096, true);
            using var jr = new JsonTextReader(tr);
            copy = jsonSerializer.Deserialize<T>(jr);

            return copy;
        }

        protected override ISerializationTarget Deserialize(Type type, MemoryStream stream)
        {
            object copy;
            stream.Position = 0;
            using var tr = new StreamReader(stream, Encoding.UTF8, false, 4096, true);
            using var jr = new JsonTextReader(tr);
            copy = jsonSerializer.Deserialize(jr, type);

            return (ISerializationTarget)copy;
        }

        #endregion

        public override string ToString()
        {
            return "Newtonsoft.Json";
        }

        public override Type GetSerializationOutPutType() => typeof(MemoryStream);
    }
}