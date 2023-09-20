using BenchmarkDotNet.Attributes;
using Serialization.Benchmarks.Abstractions;
using Serialization.Domain.Builders;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.MessagePack;
using Serialization.Services;

namespace Serialization.Benchmarks.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn] //, PerfCollectProfiler, EtwProfiler]
    public class RESTSerializationBenchmark : ISerializableBenchmark
    {
        private RestClient client = new();
        private WorkloadService parallelService => new();
        private object SerializedTarget;

        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        [ParamsSource(nameof(Targets))]
        public ISerializationTarget Target { get; set; }

        [Params(15, 100, 500)]
        public int NumHosts { get; set; }

        [Params(20, 1000)]
        public int MessagesPerSecond { get; set; }

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            new FlatBuffersSerializer(),
            new MessagePackCSharpSerializer(),
            //new SytemTextJsonSerializer(),
        };

        public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
        {
            new VideoBuilder().Generate(),
            new SocialInfoBuilder().WithSeveralComments(1000, 1000).Generate(),
        };

        [GlobalSetup]
        public void GlobalSetup() => parallelService.RunParallelProcessingAsync(Serializer, NumHosts, MessagesPerSecond);

        [Benchmark]
        public void RoundTripTime()
        {
            ISerializationTarget response;
            Target.Serialize(Serializer);

            if (Serializer.GetSerializationResult(Target.GetType(), out SerializedTarget))
            {
                response = (ISerializationTarget)client.PostAsync($"receiver/{Serializer.ToString()}", SerializedTarget).GetAwaiter().GetResult();
                response.Deserialize(Serializer);
            }
        }

        public long Serialize() => Target.Serialize(Serializer);

        [GlobalCleanup]
        public void GlobalCleanup() => Serializer.Cleanup();
    }
}
