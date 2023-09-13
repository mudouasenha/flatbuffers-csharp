using BenchmarkDotNet.Attributes;
using Serialization.Benchmarks.Abstractions;
using Serialization.Domain.Builders;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.SystemTextJson;

namespace Serialization.Benchmarks.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn] //, PerfCollectProfiler, EtwProfiler]
    public class SerializationBenchmark : ISerializableBenchmark
    {
        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        [ParamsSource(nameof(Targets))]
        public ISerializationTarget Target { get; set; }

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            //new FlatBuffersSerializerBase(),
            //new MessagePackCSharpSerializer()
            new SytemTextJsonSerializer(),
        };

        public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
        {
            new VideoBuilder().Generate(),
            new SocialInfoBuilder().Generate(),
            new SocialInfoBuilder().WithSeveralComments(1000, 1000).Generate(),
            new VideoInfoBuilder().Generate(),
            new ChannelBuilder().Generate()
        };

        [GlobalSetup(Target = nameof(Deserialize))]
        public void GlobalSetup() => Serialize();


        [Benchmark]
        public void RoundTripTime()
        {
            Serializer.BenchmarkSerialize(Target.GetType(), Target);
            Serializer.BenchmarkDeserialize(Target.GetType(), Target);
        }

        [Benchmark]
        public long Serialize() => Serializer.BenchmarkSerialize(Target.GetType(), Target);

        [Benchmark]
        public long Deserialize() => Serializer.BenchmarkDeserialize(Target.GetType(), Target);

        [GlobalCleanup]
        public void GlobalCleanup() => Serializer.Cleanup();
    }
}
