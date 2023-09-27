using Newtonsoft.Json;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;
using System.Text;

namespace Serialization.Serializers.SystemTextJson
{
    public class NewtonsoftJsonSerializer : BaseDirectSerializer<MemoryStream>
    {
        private Newtonsoft.Json.JsonSerializer jsonSerializer;

        public NewtonsoftJsonSerializer() : base()
        {
            jsonSerializer = new Newtonsoft.Json.JsonSerializer();
        }

        #region Serialization

        protected override MemoryStream Serialize<T>(T original, out long messageSize)
        {
            var stream = new MemoryStream();
            using var tw = new StreamWriter(stream, Encoding.UTF8, 1024, true);
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
            using var tw = new StreamWriter(stream, Encoding.UTF8, 1024, true);
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
            using var tr = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            using var jr = new JsonTextReader(tr);
            copy = jsonSerializer.Deserialize<T>(jr);

            return copy;
        }

        protected override ISerializationTarget Deserialize(Type type, MemoryStream stream)
        {
            object copy;
            stream.Position = 0;
            using var tr = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            using var jr = new JsonTextReader(tr);
            copy = jsonSerializer.Deserialize(jr, type);

            return (ISerializationTarget)copy;
        }

        #endregion

        public override string ToString()
        {
            return "Newtonsoft.Json";
        }

        public override bool GetSerializationResult(Type type, out object result)
        {
            if (type == typeof(Video))
            {
                result = SerializationResults[typeof(Video)].Result;
                return true;
            }
            if (type == typeof(VideoInfo))
            {
                result = SerializationResults[typeof(VideoInfo)].Result;
                return true;
            }
            if (type == typeof(SocialInfo))
            {
                result = SerializationResults[typeof(SocialInfo)].Result;
                return true;
            }
            if (type == typeof(Channel))
            {
                result = SerializationResults[typeof(Channel)].Result;
                return true;
            }
            throw new NotImplementedException($"Conversion for type {type} not implemented!");
        }

        public override Type GetSerializationOutPutType() => typeof(MemoryStream);
    }
}