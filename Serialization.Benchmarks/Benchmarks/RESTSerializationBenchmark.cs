using BenchmarkDotNet.Attributes;
using Serialization.Domain.Builders;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.MessagePack;
using Serialization.Services;

namespace Serialization.Benchmarks.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn] //, PerfCollectProfiler, EtwProfiler]
    public class SerializationNetworkingBenchmark // : ISerializableBenchmark
    {
        private RestClient client = new();
        private object SerializedTarget;

        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        [ParamsSource(nameof(Targets))]
        public ISerializationTarget Target { get; set; }

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            new FlatBuffersSerializer(),
            new MessagePackCSharpSerializer()
            //new SytemTextJsonSerializer(),
        };

        public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
        {
            new VideoBuilder().Generate(),
            new SocialInfoBuilder().Generate(),
            new SocialInfoBuilder().WithSeveralComments(1000, 1000).Generate(),
            new VideoInfoBuilder().Generate(),
            new ChannelBuilder().Generate()
        };

        [GlobalSetup(Target = nameof(RoundTripTime))]
        public async void GlobalSetup() => await client.PostAsync($"receiver/{Serializer}", SerializedTarget);

        [Benchmark]
        public void RoundTripTime()
        {
            Target.Serialize(Serializer);

            if (Serializer.GetSerializationResult(Target.GetType(), out SerializedTarget))
                client.PostAsync("receiver/video", SerializedTarget).GetAwaiter().GetResult();

            Thread.Sleep(TimeSpan.FromSeconds(0.1));
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            Serializer.Cleanup();
        }
    }
}
