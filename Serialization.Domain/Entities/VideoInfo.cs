using Avro.Specific;
using FlatBuffersModels;
using Google.Protobuf;
using Google.Protobuf.Collections;
using MessagePack;
using ProtoBuf;
using Serialization.Domain.Interfaces;
using Thrift.Protocol;

namespace Serialization.Domain.Entities
{
    [MessagePackObject]
    [Serializable]
    [ProtoContract]
    public class VideoInfo : ISerializationTarget
    {
        [NonSerialized]
        private IMessage<ProtoObjects.VideoInfo> protoObject;

        [NonSerialized]
        private thriftObjects.VideoInfo thriftObject;

        [NonSerialized]
        private avroObjects.VideoInfo avroObject;

        public VideoInfo()
        { }

        public VideoInfo(long duration, string description, long size, VideoQualityFlatModel[] qualities)
        {
            Duration = duration;
            Description = description;
            Size = size;
            Qualities = qualities;
        }

        [Key(0)]
        [ProtoMember(1)]
        public long Duration { get; set; }

        [Key(1)]
        [ProtoMember(2)]
        public string Description { get; set; }

        [Key(2)]
        [ProtoMember(3)]
        public long Size { get; set; }

        [Key(3)]
        [ProtoMember(4)]
        public VideoQualityFlatModel[] Qualities { get; set; }

        public long Serialize(ISerializer serializer) => serializer.BenchmarkSerialize(this);

        public long Deserialize(ISerializer serializer) => serializer.BenchmarkDeserialize(this);

        public bool Equals(VideoInfo other) => Duration.Equals(other.Duration) && Description.Equals(other.Description) && Size.Equals(other.Size);

        public bool Equals(ISerializationTarget other) => other is VideoInfo otherVideoInfo && Equals(otherVideoInfo);

        public override string ToString()
        {
            return "VideoInfo";
        }

        public void CreateProtobufMessage()
        {
            var qualities = new RepeatedField<ProtoObjects.VideoInfo.Types.VideoQualities>();
            qualities.AddRange(Qualities.ToArray().Select(x => (ProtoObjects.VideoInfo.Types.VideoQualities)x));

            protoObject = new ProtoObjects.VideoInfo()
            {
                Duration = (ulong)Duration,
                Description = Description,
                Size = (ulong)Size,
                Qualities = { qualities }
            };
        }

        public IMessage GetProtobufMessage()
        {
            return protoObject;
        }

        public TBase GetThriftMessage()
        {
            return thriftObject;
        }

        public void CreateThriftMessage()
        {
            var qualities = new List<thriftObjects.VideoQualities>();
            qualities.AddRange(Qualities.ToArray().Select(x => (thriftObjects.VideoQualities)x));

            thriftObject = new thriftObjects.VideoInfo()
            {
                Duration = Duration,
                Description = Description,
                Size = Size,
                Qualities = qualities
            };
        }

        public ISpecificRecord GetAvroMessage()
        {
            return avroObject;
        }

        public void CreateAvroMessage()
        {
            var qualities = Qualities.Select(x => (avroObjects.VideoQualities)x).ToArray();

            avroObject = new avroObjects.VideoInfo()
            {
                Duration = Duration,
                Description = Description,
                Size = Size,
                Qualities = qualities
            };
        }
    }
}