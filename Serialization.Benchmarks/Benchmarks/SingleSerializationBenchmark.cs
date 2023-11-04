using BenchmarkDotNet.Attributes;
using Serialiazation.Serializers.Manual;
using Serialization.Benchmarks.Abstractions;
using Serialization.Domain;
using Serialization.Domain.Builders;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.ApacheAvro;
using Serialization.Serializers.CapnProto;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.MessagePack;
using Serialization.Serializers.SystemTextJson;

namespace Serialization.Benchmarks.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn] //, PerfCollectProfiler, EtwProfiler]
    public class SingleSerializationBenchmark : ISerializableBenchmark
    {
        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        [ParamsSource(nameof(Targets))]
        public ISerializationTarget Target { get; set; }

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            new FlatBuffersSerializer(),
            new MessagePackCSharpSerializer(),
            new NewtonsoftJsonSerializer(),
            new BinaryFormatterSerializer(),
            new ProtobufSerializer(),
            new ApacheThriftSerializer(),
            new ApacheAvroSerializer(),
            new CapnProtoSerializer(),
        };

        public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
        {
            new VideoBuilder().Generate(),
            new SocialInfoBuilder().Generate(),
            new VideoInfoBuilder().Generate(),
            new ChannelBuilder().Generate()
        };

        [IterationSetup(Target = nameof(Serialize))]
        public void SetupSerialize()
        {
            if (Serializer is ProtobufSerializer)
                Target.CreateProtobufMessage();

            if (Serializer is ApacheThriftSerializer)
                Target.CreateThriftMessage();

            if (Serializer is ApacheAvroSerializer)
                Target.CreateAvroMessage();

            if (Serializer is CapnProtoSerializer)
                Target.CreateCapnProtoMessage();
        }

        [IterationSetup(Target = nameof(Deserialize))]
        public void SetupDeserialize()
        {
            SetupSerialize();
            Serialize();
        }

        [Benchmark]
        public long Serialize() => Serializer.BenchmarkSerialize(Target.GetType(), Target);

        [Benchmark]
        public long Deserialize() => Serializer.BenchmarkDeserialize(Target.GetType(), Target);

        [IterationSetup]
        public void GlobalCleanup() => Serializer.Cleanup();
    }
}