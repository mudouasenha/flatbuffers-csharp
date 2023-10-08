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

        [Params(128, 512, 4096, 6400, 12800, 32768, 40960, 64000, 102400, 128000)]
        //[Params(4096, 6400, 32768, 64000, 102400, 262144, 409600, 655360, 1376256, 2097152)]
        public int NumMessages { get; set; }

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            new FlatBuffersSerializer(),
            new MessagePackCSharpSerializer(),
            new NewtonsoftJsonSerializer(),
            new ManualSerializer()
        };

        public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
        {
            new VideoBuilder().Generate(),
            new SocialInfoBuilder().Generate(),
            //new SocialInfoBuilder().WithSeveralComments(1000, 1000).Generate(),
            new VideoInfoBuilder().Generate(),
            new ChannelBuilder().Generate()
        };

        [GlobalSetup(Target = nameof(DeserializeParallelMakespan))]
        public void SetupDeserialize()
        {
            TargetList = new List<ISerializationTarget>();
            TargetList = GenerateSerializationTargets(NumMessages);
            SerializeParallel(TargetList);
        }

        [GlobalSetup(Target = nameof(SerializeParallelMakespan))]
        public void SetupSerialize()
        {
            TargetList = new List<ISerializationTarget>();
            TargetList = GenerateSerializationTargets(NumMessages);
        }

        //[Benchmark]
        //public long Serialize() => Serializer.BenchmarkSerialize(Target.GetType(), Target);

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

        [Benchmark]
        public long DeserializeParallelMakespan()
        {
            var tasks = new List<Task>();

            //var targets = GenerateSerializationTargets(NumMessages);
            //SerializeParallel(targets);

            var stopwatch = Stopwatch.StartNew();

            List<List<ISerializationTarget>> partitions = PartitionList(TargetList, NumThreads);

            foreach (var partition in partitions)
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

            //var targets = GenerateSerializationTargets(NumMessages);
            //SerializeParallel(targets);

            var stopwatch = Stopwatch.StartNew();

            List<List<ISerializationTarget>> partitions = PartitionList(TargetList, NumThreads);

            foreach (var partition in partitions)
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

        //[Benchmark]
        //public void SerializeParallelMakespan()
        //{
        //    var tasks = new List<Task>();

        //    int messagesPerThread = NumMessages / NumThreads;

        //    for (int i = 0; i < NumThreads; i++)
        //    {
        //        tasks.Add(Task.Run(() =>
        //        {
        //            for (int j = 0; j < messagesPerThread; j++)
        //            {
        //                Serializer.BenchmarkSerialize(Target.GetType(), Target);
        //            }
        //        }));
        //    }

        //    Task.WhenAll(tasks).Wait();
        //}



        [GlobalCleanup]
        public void GlobalCleanup() => Serializer.Cleanup();

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

        public void SerializeParallel(List<ISerializationTarget> targets)
        {
            var tasks = new List<Task>();
            List<List<ISerializationTarget>> partitions = PartitionList(targets, NumThreads);

            foreach (var partition in partitions)
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
    }
}
