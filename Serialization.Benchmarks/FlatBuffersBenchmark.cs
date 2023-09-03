using BenchmarkDotNet.Attributes;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.SystemTextJson;
using Serialization.Services;

namespace Serialization.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn]
    public class FlatBuffersBenchmark : ISerializableBenchmark
    {
        [ParamsSource(nameof(Serializers))]
        public ISerializer<Video> Serializer { get; set; }

        private VideoFlatBuffersSerializer _videoconverter = new();
        private RestClient client = new();
        private Video vid;
        private byte[] vidSerialized;

        public IEnumerable<ISerializer<Video>> Serializers => new ISerializer<Video>[]
        {
            new VideoFlatBuffersSerializer(),
            new VideoSytemTextJsonSerializer()
        };

        [GlobalSetup]
        public void GlobalSetup()
        {
            vid = new VideoService().CreateVideo();
            vidSerialized = Serializer.Serialize(vid);
        }

        [Benchmark]
        public void FullProcess() => Serializer.Deserialize(Serializer.Serialize(vid));

        [Benchmark]
        public void Serialization() => Serializer.Serialize(vid);

        [Benchmark]
        public void Deserialization() => Serializer.Deserialize(vidSerialized);

        [Benchmark]
        public void Rest_FullProcess_Multiple()
        {
            client.PostAsync("receiver/video", Serializer.Serialize(vid)).GetAwaiter().GetResult();
            Thread.Sleep(TimeSpan.FromSeconds(0.1));
        }

        [Benchmark]
        public void Rest_FullProcess() => client.PostAsync("receiver/video", Serializer.Serialize(vid)).GetAwaiter().GetResult();

    }
}
