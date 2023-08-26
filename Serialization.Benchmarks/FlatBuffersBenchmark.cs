using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Serialization.Domain.Entities;
using Serialization.Serializers.FlatBuffers;
using Serialization.Services;

namespace Serialization.Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class FlatBuffersBenchmark
    {
        [Params(10, 1000, 1000000)]
        public int IterationCount { get; set; }


        private VideoService _videoService = new();
        private VideoFlatBuffersConverter _videoconverter = new();
        private RestClient client = new();
        private Video vid;
        private byte[] vidSerialized;
        private IServiceProvider serviceProvider;

        [GlobalSetup]
        public void GlobalSetup()
        {
            vid = _videoService.CreateVideo();
            vidSerialized = _videoconverter.Serialize(vid);
        }

        [Benchmark]
        public void FullProcess() => _videoconverter.Deserialize(_videoconverter.Serialize(vid));

        [Benchmark]
        public void Serialization() => _videoconverter.Serialize(vid);

        [Benchmark]
        public void Serialization_Multiple()
        {
            for (int i = 0; i <= IterationCount; i++) _videoconverter.Serialize(vid);
        }

        [Benchmark]
        public async Task MeasureRestLatency() => await client.PostAsync("receiver/video", vidSerialized);

        [Benchmark]
        public void Deserialization_Multiple()
        {
            for (int i = 0; i <= IterationCount; i++) _videoconverter.Deserialize(vidSerialized);
        }

        [Benchmark]
        public void Deserialization() => _videoconverter.Deserialize(vidSerialized);
    }
}
