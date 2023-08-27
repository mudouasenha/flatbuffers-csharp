using BenchmarkDotNet.Attributes;
using Serialization.Domain.Entities;
using Serialization.Serializers.FlatBuffers;
using Serialization.Services;

namespace Serialization.Benchmarks
{
    public class FlatBuffersBenchmark
    {
        [Params(1, 10, 1_000, 100_000)]
        public int IterationCount { get; set; }

        [Params(2, 4, 8)] // Number of concurrent threads
        public int ThreadCount { get; set; }

        private object _lock = new object();

        private VideoService _videoService = new();
        private VideoFlatBuffersConverter _videoconverter = new();
        private RestClient client = new();
        private Video vid;
        private byte[] vidSerialized;

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
        public void Deserialization() => _videoconverter.Deserialize(vidSerialized);

        [Benchmark]
        public byte[] MeasureRestLatency() => client.PostAsync("receiver/video", vidSerialized).Result;

        [Benchmark]
        public void Serialization_Multiple()
        {
            for (int i = 0; i <= IterationCount; i++) _videoconverter.Serialize(vid);
        }

        [Benchmark]
        public void Deserialization_Multiple()
        {
            for (int i = 0; i <= IterationCount; i++) _videoconverter.Deserialize(vidSerialized);
        }

        [Benchmark]
        public void HighWorkloadSerialization()
        {
            Parallel.For(0, ThreadCount, _ =>
            {
                lock (_lock)
                {
                    _videoconverter.Serialize(vid);
                }
            });
        }
    }
}
