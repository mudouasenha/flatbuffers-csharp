using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.MessagePack;
using Serialization.Serializers.SystemTextJson;
using Serialization.Services;

namespace Serialization.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn, PerfCollectProfiler, EtwProfiler]
    public class SerializationNetworkingBenchmark // : ISerializableBenchmark
    {
        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        [ParamsSource(nameof(Targets))]
        public ISerializationTarget Target { get; set; }

        private RestClient client = new();
        private object SerializedTarget;

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            new VideoFlatBuffersSerializer(),
            new SytemTextJsonSerializer(),
            new MessagePackCSharpSerializer()
        };

        public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
        {
            new VideoService().CreateVideo()
        };

        [Benchmark]
        public void Rest_FullProcess_Multiple()
        {
            Target.Serialize(Serializer);

            if (Serializer.GetSerializationResult(Target.GetType(), out SerializedTarget))
                client.PostAsync("receiver/video", SerializedTarget).GetAwaiter().GetResult();

            Thread.Sleep(TimeSpan.FromSeconds(0.1));
        }

        [Benchmark]
        public void Rest_FullProcess()
        {
            Target.Serialize(Serializer);

            if (Serializer.GetSerializationResult(Target.GetType(), out SerializedTarget))
                client.PostAsync("receiver/video", SerializedTarget).GetAwaiter().GetResult();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            Serializer.Cleanup();
        }
    }
}
