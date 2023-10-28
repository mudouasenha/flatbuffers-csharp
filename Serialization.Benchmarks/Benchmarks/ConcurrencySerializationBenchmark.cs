using BenchmarkDotNet.Attributes;
using Serialiazation.Serializers.Manual;
using Serialization.Domain.Builders;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.MessagePack;
using Serialization.Serializers.SystemTextJson;
using System.Diagnostics;

namespace Serialization.Benchmarks.Benchmarks
{
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn]
    public class ConcurrencySerializationBenchmark //: ISerializableBenchmark
    {
        public List<ISerializationTarget> TargetList;

        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        [ParamsSource(nameof(Targets))]
        public ISerializationTarget Target { get; set; }

        [Params(2, 4, 8, 16)]
        public int NumThreads { get; set; }

        [Params(128_000, 256_000, 384_000, 512_000, 640_000, 768_000, 896_000, 1_024_000, 1_152_000, 1_280_000)]
        public int NumMessages { get; set; }

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            new FlatBuffersSerializer(),
            new MessagePackCSharpSerializer(),
            new NewtonsoftJsonSerializer(),
            new BinaryFormatterSerializer()
        };

        public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
        {
            new VideoBuilder().Generate(),
            new SocialInfoBuilder().Generate(),
            //new SocialInfoBuilder().WithSeveralComments(1000, 1000).Generate(),
            new VideoInfoBuilder().Generate(),
            new ChannelBuilder().Generate()
        };

        [IterationSetup(Target = nameof(DeserializeParallelMakespan))]
        public void SetupDeserialize()
        {
            SetupSerialize();
            SerializeParallel(TargetList);
        }

        [IterationSetup(Target = nameof(SerializeParallelMakespan))]
        public void SetupSerialize()
        {
            TargetList = new List<ISerializationTarget>();
            TargetList = GenerateSerializationTargets(NumMessages);
            GenerateProtobufMessages();
        }

        [Benchmark]
        public long DeserializeParallelMakespan()
        {
            var tasks = new List<Task>();

            var stopwatch = Stopwatch.StartNew();

            foreach (var partition in PartitionList(TargetList, NumThreads))
            {
                tasks.Add(Task.Run(() =>
                {
                    foreach (var message in partition)
                    {
                        Serializer.BenchmarkDeserialize(message.GetType(), message);
                    }
                }));
            }

            Task.WhenAll(tasks).Wait();

            stopwatch.Stop();

            return (long)stopwatch.ElapsedTicks / Stopwatch.Frequency;
        }


        [Benchmark]
        public long SerializeParallelMakespan()
        {
            var tasks = new List<Task>();

            var stopwatch = Stopwatch.StartNew();

            foreach (var partition in PartitionList(TargetList, NumThreads))
            {
                tasks.Add(Task.Run(() =>
                {
                    foreach (var message in partition)
                    {
                        Serializer.BenchmarkSerialize(message.GetType(), message);
                    }
                }));
            }

            Task.WhenAll(tasks).Wait();

            stopwatch.Stop();

            return (long)stopwatch.ElapsedTicks / Stopwatch.Frequency;
        }

        [IterationCleanup]
        public void IterationCleanup() => Serializer.Cleanup();

        private static List<List<T>> PartitionList<T>(List<T> list, int partitions)
        {
            List<List<T>> result = new List<List<T>>();
            int itemsPerPartition = list.Count / partitions;

            for (int i = 0; i < partitions; i++)
            {
                int startIndex = i * itemsPerPartition;
                int endIndex = (i == partitions - 1) ? list.Count : startIndex + itemsPerPartition;
                result.Add(list.GetRange(startIndex, endIndex - startIndex));
            }

            return result;
        }

        private void SerializeParallel(List<ISerializationTarget> targets)
        {
            var tasks = new List<Task>();
            foreach (var partition in PartitionList(targets, NumThreads))
            {
                tasks.Add(Task.Run(() =>
                {
                    foreach (var message in partition)
                    {
                        Serializer.BenchmarkSerialize(message.GetType(), message);
                    }
                }));
            }

            Task.WhenAll(tasks).Wait();
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
