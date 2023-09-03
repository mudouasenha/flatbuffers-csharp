using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;
using System.Text;
using System.Text.Json;

namespace Serialization.Serializers.SystemTextJson
{
    public class VideoSytemTextJsonSerializer : ISerializer<Video>
    {
        public Video Deserialize(byte[] buf)
        {
            using var stream = new MemoryStream(buf);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            var jsonString = reader.ReadToEnd();
            return JsonSerializer.Deserialize<Video>(jsonString);
        }

        public byte[] Serialize(Video obj)
        {
            var jsonString = JsonSerializer.Serialize(obj);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Flush();
            return stream.ToArray();
        }
    }
}