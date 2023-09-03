using BenchmarkDotNet.Attributes;
using Bogus;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.SystemTextJson;
using Serialization.Services;
using System.Numerics;

namespace Serialization.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn]
    public class FlatBuffersBenchmark : ISerializableBenchmark
    {
        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        [ParamsSource(nameof(Targets))]
        public ISerializable Target { get; set; }

        private RestClient client = new();
        private Video vid;
        private ISerializable vidSerialized;

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            new VideoFlatBuffersSerializer(),
            new VideoSytemTextJsonSerializer()
        };

        public IEnumerable<ISerializable> Targets => new ISerializable[]
        {
            new VideoService().CreateVideo()
        };

        [GlobalSetup]
        public void GlobalSetup()
        {
            vid = new VideoService().CreateVideo();
            vidSerialized = vid.Serialize<ISerializable>(Serializer);
        }

        [Benchmark]
        public void FullProcess() => vid.Serialize<ISerializable>(Serializer).Deserialize(Serializer);

        [Benchmark]
        public void Serialization() => vid.Serialize<ISerializable>(Serializer);

        [Benchmark]
        public void Deserialization() => vidSerialized.Deserialize(Serializer);

        [Benchmark]
        public void Rest_FullProcess_Multiple()
        {
            client.PostAsync("receiver/video", vid.Serialize<byte[]>(Serializer)).GetAwaiter().GetResult();
            Thread.Sleep(TimeSpan.FromSeconds(0.1));
        }

        [Benchmark]
        public void Rest_FullProcess() => client.PostAsync("receiver/video", vid.Serialize<byte[]>(Serializer)).GetAwaiter().GetResult();

    }
}
