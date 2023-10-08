using FlatBuffersModels;
using MessagePack;
using Serialization.Domain.Interfaces;

namespace Serialization.Domain.Entities
{

    [MessagePackObject]
    [Serializable]
    public class VideoInfo : ISerializationTarget
    {
        public VideoInfo() { }

        public VideoInfo(int duration, string description, long size, VideoQualityFlatModel[] qualities)
        {
            Duration = duration;
            Description = description;
            Size = size;
            Qualities = qualities;
        }

        [Key(0)]
        public int Duration { get; set; }

        [Key(1)]
        public string Description { get; set; }

        [Key(2)]
        public long Size { get; set; }

        [Key(3)]
        public VideoQualityFlatModel[] Qualities { get; set; }

        public long Serialize(ISerializer serializer) => serializer.BenchmarkSerialize(this);

        public long Deserialize(ISerializer serializer) => serializer.BenchmarkDeserialize(this);

        public bool Equals(VideoInfo other) => Duration.Equals(other.Duration) && Description.Equals(other.Description) && Size.Equals(other.Size);

        public bool Equals(ISerializationTarget other) => other is VideoInfo otherVideoInfo && Equals(otherVideoInfo);

        public override string ToString()
        {
            return "VideoInfo";
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