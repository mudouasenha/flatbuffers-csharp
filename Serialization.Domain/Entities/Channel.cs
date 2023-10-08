using MessagePack;
using Serialization.Domain.Interfaces;

namespace Serialization.Domain.Entities
{

    [MessagePackObject]
    [Serializable]
    public class Channel : ISerializationTarget
    {
        public Channel() { }
        public Channel(string name, int subscribers, int channelId)
        {
            Name = name;
            Subscribers = subscribers;
            ChannelId = channelId;
        }

        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public int Subscribers { get; set; }

        [Key(2)]
        public int ChannelId { get; set; }

        public long Serialize(ISerializer serializer) => serializer.BenchmarkSerialize(this);

        public long Deserialize(ISerializer serializer) => serializer.BenchmarkDeserialize(this);

        public bool Equals(Channel other) => Name.Equals(other.Name) && Subscribers.Equals(other.Subscribers) && ChannelId.Equals(other.ChannelId);

        public bool Equals(ISerializationTarget other) => other is Channel otherChannel && Equals(otherChannel);

        public override string ToString()
        {
            return "Channel";
        }

        public long Serialize(ref byte[] target)
        {
            throw new NotImplementedException();
        }

        public long Deserialize(ref byte[] target)
        {
            throw new NotImplementedException();
        }
    }
}