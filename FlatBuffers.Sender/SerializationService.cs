using FlatBuffers.Sender.Video;
using Google.FlatBuffers;

namespace FlatBuffers.Sender
{
    public class SerializationService : ISerializationService
    {
        private readonly FlatBufferBuilder _flatBufferBuilder;

        public SerializationService()
        {
            _flatBufferBuilder = new FlatBufferBuilder(1024);
        }

        public void Deserialize()
        {
            var ch = Channel.StartChannel;
        }

        public void Serialize()
        {
        }
    }
}