using BenchmarkDotNet.Attributes;
using Flurl;
using Serialization.Domain;
using Serialization.Domain.Builders;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.ApacheAvro;
using Serialization.Serializers.CapnProto;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.MessagePack;
using Serialization.Serializers.SystemTextJson;
using Serialization.Services;
using Serialization.Services.CsvExporter;
using System.Diagnostics;

namespace Serialization.Benchmarks.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn] //, PerfCollectProfiler, EtwProfiler]
    public class RESTSerializationBenchmark
    {
        private RestClient client = new();
        private ExecutionInfo executionInfo;
        private static string BenchmarkDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        private const string fileName = "Measurements-REST-Client-SerializationBenchmark.csv";
        private readonly long epochTicks = new DateTime(1970, 1, 1).Ticks;

        private WorkloadService parallelService => new();

        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        //[ParamsSource(nameof(Targets))]
        public ISerializationTarget Target { get; set; }

        [Params(64, 128/*, 192/*, 256, 320/*, 384, 448, 512, 576, 640*/)]
        public int NumThreads { get; set; }

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            new FlatBuffersSerializer(),
            new MessagePackCSharpSerializer(),
            new NewtonsoftJsonSerializer(),
            //new BinaryFormatterSerializer(),
            new ProtobufSerializer(),
            new ApacheThriftSerializer(),
            new ApacheAvroSerializer(),
            new CapnProtoSerializer(),
        };

        public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
        {
        };

        private ISerializationTarget GenerateRandomTarget()
        {
            var random = new Random().Next(4);
            return random switch
            {
                0 => new ChannelBuilder().Generate(),
                1 => new SocialInfoBuilder().Generate(),
                2 => new VideoInfoBuilder().Generate(),
                3 => new VideoBuilder().Generate(),
                _ => new ChannelBuilder().Generate(),
            };
        }

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            //await parallelService.DispatchAsync(Serializer, null, NumThreads);
            executionInfo = new ExecutionInfo(Target.ToString(), Serializer.ToString(), 0, NumThreads);
        }

        [IterationSetup(Target = nameof(Latency_RTT))]
        public void IterationSetup()
        {
            Target = GenerateRandomTarget();

            if (Serializer is ProtobufSerializer)
                Target.CreateProtobufMessage();
            if (Serializer is ApacheThriftSerializer)
                Target.CreateThriftMessage();
            if (Serializer is ApacheAvroSerializer)
                Target.CreateAvroMessage();
            if (Serializer is CapnProtoSerializer)
                Target.CreateCapnProtoMessage();

            Target.Serialize(Serializer);
        }

        [Benchmark]
        public void Latency_RTT()
        {
            if (Serializer.GetSerializationResult(Target.GetType(), out var serializedTarget))
            {
                var stopwatch = Stopwatch.StartNew();
                var response = client.PostAsync("receiver/serializer"
                                .SetQueryParam("serializerType", Serializer.ToString())
                                .SetQueryParam("serializationType", Target.GetType().Name), serializedTarget).GetAwaiter().GetResult();
                stopwatch.Stop();
                var result = new MeasurementRest(stopwatch.ElapsedMilliseconds, DateTimeOffset.Now.ToUnixTimeMilliseconds(), nameof(Latency_RTT));
                executionInfo.Measurements.Add(result);
            }
        }

        public long Serialize() => Target.Serialize(Serializer);

        [GlobalCleanup]
        public async Task GlobalCleanup()
        {
            CsvExporter.ExportMeasurementsREST(fileName, executionInfo);
            //await parallelService.ClearAsync();
            //await parallelService.SaveServerResultsAsync(BenchmarkDateTime, Serializer.ToString(), Target.GetType().Name, NumThreads, null);
            Serializer.Cleanup();
            await Task.Delay(500);
        }
    }
}