using BenchmarkDotNet.Attributes;
using Serialization.Domain;
using Serialization.Domain.Builders;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.MessagePack;
using Serialization.Serializers.SystemTextJson;
using Serialization.Services;

namespace Serialization.Benchmarks.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn] //, PerfCollectProfiler, EtwProfiler]
    public class RESTSerializationBenchmark
    {
        private RestClient client = new();
        private readonly long epochTicks = new DateTime(1970, 1, 1).Ticks;

        private WorkloadService parallelService => new();

        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        [ParamsSource(nameof(Targets))]
        public ISerializationTarget Target { get; set; }

        [Params(10, 50, 100, 500)]
        public int NumThreads { get; set; }

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            new FlatBuffersSerializer(),
            new MessagePackCSharpSerializer(),
            new NewtonsoftJsonSerializer(),
            new ProtobufSerializer()
        };

        public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
        {
            new VideoBuilder().Generate(),
            new SocialInfoBuilder().Generate(),
            new VideoInfoBuilder().Generate(),
            new ChannelBuilder().Generate()
        };

        [GlobalSetup]
        public async Task GlobalSetup() => await parallelService.DispatchAsync(Serializer, Target.ToString(), NumThreads);

        [IterationSetup]
        public void IterationSetup()
        {
            Target.Serialize(Serializer);
            Target.CreateProtobufMessage();
        }

        [Benchmark]
        public long Latency()
        {
            if (Serializer.GetSerializationResult(Target.GetType(), out var serializedTarget))
            {
                //var response = (ISerializationTarget)client.PostAsync($"receiver/serializer", serializedTarget).GetAwaiter().GetResult();
                var response = client.PostAsync($"receiver/serializer", serializedTarget).GetAwaiter().GetResult();
                //response.Deserialize(Serializer);
            }

            return (DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerSecond;
        }

        public long Serialize() => Target.Serialize(Serializer);

        [GlobalCleanup]
        public void GlobalCleanup() => Serializer.Cleanup();
    }
}
