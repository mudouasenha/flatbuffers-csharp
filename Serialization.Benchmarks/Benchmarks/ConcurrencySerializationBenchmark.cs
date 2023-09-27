using BenchmarkDotNet.Attributes;
using Serialization.Benchmarks.Abstractions;
using Serialization.Domain.Builders;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.MessagePack;
using Serialization.Services;

namespace Serialization.Benchmarks.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn]
    public class ConcurrencySerializationBenchmark : ISerializableBenchmark
    {
        private WorkloadService parallelService => new();

        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        [Params(2, 4, 8)]
        public int NumThreads { get; set; }

        [Params(1000, 10000, 1000000)]
        public int NumMessages { get; set; }

        [ParamsSource(nameof(Targets))]
        public ISerializationTarget Target { get; set; }

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            new FlatBuffersSerializer(),
            new MessagePackCSharpSerializer(),
            //new NewtonsoftJsonSerializer(),
        };

        public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
        {
            new VideoBuilder().Generate(),
            new SocialInfoBuilder().Generate(),
            //new SocialInfoBuilder().WithSeveralComments(1000, 1000).Generate(),
            new VideoInfoBuilder().Generate(),
            new ChannelBuilder().Generate()
        };

        //[GlobalSetup]
        //public async Task GlobalSetup() => await parallelService.DispatchAsync(Serializer, NumThreads, NumMessages);

        [GlobalSetup(Target = nameof(Deserialize))]
        public void SetupDeserialize() => Serialize();

        //[Benchmark]
        //public void RoundTripTime()
        //{
        //    Serializer.BenchmarkSerialize(Target.GetType(), Target);
        //    Serializer.BenchmarkDeserialize(Target.GetType(), Target);
        //}

        [Benchmark]
        public long Serialize() => Serializer.BenchmarkSerialize(Target.GetType(), Target);

        [Benchmark]
        public long Deserialize() => Serializer.BenchmarkDeserialize(Target.GetType(), Target);

        [GlobalCleanup]
        public void GlobalCleanup() => Serializer.Cleanup();
    }
}
