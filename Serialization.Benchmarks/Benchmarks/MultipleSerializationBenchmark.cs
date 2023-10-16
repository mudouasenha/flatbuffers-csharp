﻿using BenchmarkDotNet.Attributes;
using Serialization.Domain.Builders;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.MessagePack;
using Serialization.Serializers.SystemTextJson;
using System.Diagnostics;

namespace Serialization.Benchmarks.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn] //, PerfCollectProfiler, EtwProfiler]
    public class MultipleSerializationBenchmark //: ISerializableBenchmark
    {
        public List<ISerializationTarget> TargetList;
        //private List<ExecutionInfo> executionInfoList;
        //private CsvExporter csvExporter = new();
        //private const string fileName = "Measurements-MultipleSerializationBenchmark.csv";


        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        [ParamsSource(nameof(Targets))]
        public ISerializationTarget Target { get; set; }

        [Params(64000, 128000, 192000, 256000, 320000, 384000, 448000, 512000, 576000, 640000)]
        //[Params(128, 512, 4096, 6400, 12800, 32768, 40960, 64000, 102400, 128000)]
        //[Params(4096, 6400, 32768, 64000, 102400, 262144, 409600, 655360, 1376256, 2097152)]
        public int NumMessages { get; set; }

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            new FlatBuffersSerializer(),
            new MessagePackCSharpSerializer(),
            new NewtonsoftJsonSerializer(),
            //new ManualSerializer()
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
            GenerateProtobufMessages();
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
    }
}
