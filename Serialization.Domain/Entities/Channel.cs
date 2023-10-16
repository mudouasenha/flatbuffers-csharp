using Google.Protobuf;
using MessagePack;
using ProtoBuf;
using Serialization.Domain.Interfaces;

namespace Serialization.Domain.Entities
{

    [MessagePackObject]
    [Serializable]
    [ProtoContract]
    public class Channel : ISerializationTarget
    {
        [NonSerialized]
        private IMessage<ProtoObjects.Channel> protoObject;

        public Channel() { }
        public Channel(string name, int subscribers, string channelId)
        {
            Name = name;
            Subscribers = subscribers;
            ChannelId = channelId;
        }

        [Key(0)]
        [ProtoMember(1)]
        public string Name { get; set; }

        [Key(1)]
        [ProtoMember(2)]
        public int Subscribers { get; set; }

        [Key(2)]
        [ProtoMember(4)]
        public string ChannelId { get; set; }

        public long Serialize(ISerializer serializer) => serializer.BenchmarkSerialize(this);

        public long Deserialize(ISerializer serializer) => serializer.BenchmarkDeserialize(this);

        public bool Equals(Channel other) => Name.Equals(other.Name) && Subscribers.Equals(other.Subscribers) && ChannelId.Equals(other.ChannelId);

        public bool Equals(ISerializationTarget other) => other is Channel otherChannel && Equals(otherChannel);

        public override string ToString()
        {
            return "Channel";
        }

        public void CreateProtobufMessage()
        {
            protoObject = new ProtoObjects.Channel()
            {
                ChannelId = ChannelId,
                Name = Name,
                Subscribers = (uint)Subscribers
            };
        }

        public IMessage GetProtobufMessage()
        {
            return protoObject;
        }
    }
}