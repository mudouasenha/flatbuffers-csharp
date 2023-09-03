using Serialization.Domain.Interfaces;
using System.Text;
using System.Text.Json;

namespace Serialization.Serializers.SystemTextJson
{
    public class SytemTextJsonSerializer : BaseDirectSerializer<MemoryStream>
    {
        protected override MemoryStream Serialize<T>(T original, out long messageSize)
        {
            var jsonString = JsonSerializer.Serialize(original);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Flush();

            messageSize = stream.Position;
            return stream;
        }

        protected override ISerializable Deserialize<T>(MemoryStream buf)
        {
            T copy;
            using var reader = new StreamReader(buf, Encoding.UTF8);
            var jsonString = reader.ReadToEnd();
            copy = JsonSerializer.Deserialize<T>(jsonString);

            return (ISerializable)copy;
        }

        protected override MemoryStream Serialize(Type type, MemoryStream original, out long messageSize)
        {
            var jsonString = JsonSerializer.Serialize(original); // TODO: resolve type
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Flush();

            messageSize = stream.Position;
            return stream;
        }

        protected override ISerializable Deserialize(Type type, MemoryStream serializedObject)
        {
            object copy;
            using var reader = new StreamReader(serializedObject, Encoding.UTF8);
            var jsonString = reader.ReadToEnd();
            copy = JsonSerializer.Deserialize(jsonString, type);

            return (ISerializable)copy;
        }
    }
}