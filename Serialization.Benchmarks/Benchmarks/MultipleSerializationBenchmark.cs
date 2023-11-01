using BenchmarkDotNet.Attributes;
using Serialization.Domain;
using Serialization.Domain.Builders;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.ApacheAvro;
using System.Diagnostics;

namespace Serialization.Benchmarks.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn] //, PerfCollectProfiler, EtwProfiler]
    public class MultipleSerializationBenchmark
    {
        public List<ISerializationTarget> TargetList;
        //private List<ExecutionInfo> executionInfoList;
        //private CsvExporter csvExporter = new();
        //private const string fileName = "Measurements-MultipleSerializationBenchmark.csv";

        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        [ParamsSource(nameof(Targets))]
        public ISerializationTarget Target { get; set; }

        [Params(128_000, 256_000, 384_000, 512_000, 640_000, 768_000, 896_000, 1_024_000, 1_152_000, 1_280_000)]
        public int NumMessages { get; set; }

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            //new FlatBuffersSerializer(),
            //new MessagePackCSharpSerializer(),
            //new NewtonsoftJsonSerializer(),
            //new BinaryFormatterSerializer(),
            new ProtobufSerializer(),
            //new ApacheThriftSerializer(),
            //new ApacheAvroSerializer(),
            //new CapnProtoSerializer(),
        };

        public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
        {
            new VideoBuilder().Generate(),
            new SocialInfoBuilder().Generate(),
            //new SocialInfoBuilder().WithSeveralComments(1000, 1000).Generate(),
            new VideoInfoBuilder().Generate(),
            new ChannelBuilder().Generate()
        };

        [IterationSetup(Target = nameof(DeserializeSerialMakespan))]
        public void SetupDeserialize()
        {
            SetupSerialize();
            SerializeSerial(TargetList);
        }

        [IterationSetup(Target = nameof(SerializeSerialMakespan))]
        public void SetupSerialize()
        {
            TargetList = new List<ISerializationTarget>();
            TargetList = GenerateSerializationTargets(NumMessages);

            if (Serializer is ProtobufSerializer)
                GenerateProtobufMessages();
            if (Serializer is ApacheThriftSerializer)
                GenerateThriftMessages();
            if (Serializer is ApacheAvroSerializer)
                GenerateAvroMessages();
        }

        [Benchmark]
        public long DeserializeSerialMakespan()
        {
            var stopwatch = Stopwatch.StartNew();

            foreach (var message in TargetList)
            {
                Serializer.BenchmarkDeserialize(message.GetType(), message);
            }

            stopwatch.Stop();

            return (long)stopwatch.ElapsedTicks / Stopwatch.Frequency;
        }

        [Benchmark]
        public long SerializeSerialMakespan()
        {
            var stopwatch = Stopwatch.StartNew();

            foreach (var message in TargetList)
            {
                Serializer.BenchmarkSerialize(message.GetType(), message);
            }

            stopwatch.Stop();

            return (long)stopwatch.ElapsedTicks / Stopwatch.Frequency;
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            Serializer.Cleanup();
            //SerializeSerialWithMeasurements();
            //csvExporter.ExportToCsv(fileName);
        }

        private void SerializeSerial(List<ISerializationTarget> targets)
        {
            foreach (var message in targets)
            {
                Serializer.BenchmarkSerialize(message.GetType(), message);
            }
        }

        ////[Benchmark]
        //public ExecutionInfo SerializeSerialWithMeasurements()
        //{
        //    var stopwatch = Stopwatch.StartNew();
        //    var execution = new ExecutionInfo(Target.ToString(), Serializer.ToString(), NumMessages, 0);

        //    foreach (var message in TargetList)
        //    {
        //        var stopwatch1 = Stopwatch.StartNew();
        //        var size = Serializer.BenchmarkSerialize(message.GetType(), message);
        //        stopwatch1.Stop();
        //        var result = new Measurement(stopwatch.Elapsed.Ticks, size);
        //        execution.Measurements.Add(result);
        //    }
        //    stopwatch.Stop();

        //    csvExporter.AddExecutionInfo(execution);
        //    return execution;
        //}

        private List<ISerializationTarget> GenerateSerializationTargets(int count)
        {
            var targets = new List<ISerializationTarget>(count);
            List<ISerializationTarget> subList = new();
            int itemCount = 64;

            if (count % itemCount != 0) throw new ArgumentException("count must be a multiple of 64");

            int loopCount = count <= itemCount ? 1 : count / itemCount;

            if (Target is VideoInfo) subList.AddRange(new VideoInfoBuilder().Generate(itemCount));
            else if (Target is Channel) subList.AddRange(new ChannelBuilder().Generate(itemCount));
            else if (Target is SocialInfo) subList.AddRange(new SocialInfoBuilder().Generate(itemCount));
            else if (Target is Video) subList.AddRange(new VideoBuilder().Generate(itemCount));
            else throw new ArgumentException("Unsupported target type");

            for (int i = 0; i < loopCount; i++) targets.AddRange(subList);

            return targets;
        }

        private void GenerateProtobufMessages()
        {
            foreach (var target in TargetList)
            {
                target.CreateProtobufMessage();
            }
        }

        private void GenerateThriftMessages()
        {
            foreach (var target in TargetList)
            {
                target.CreateThriftMessage();
            }
        }

        private void GenerateAvroMessages()
        {
            foreach (var target in TargetList)
            {
                target.CreateAvroMessage();
            }
        }
    }
}