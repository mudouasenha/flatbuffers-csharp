
using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using Serialization.Domain.Entities;
using Serialization.Serializers.FlatBuffers;
using Serialization.Services;

namespace Serialization.Benchmarks
{
    [RPlotExporter]
    [Config(typeof(AntiVirusFriendlyConfig))]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    [ThreadingDiagnoser]
    public class FlatBuffersBenchmark
    {
        public IEnumerable<int> NumObjectsConfig => new[] { 10, 1000, 1000000 };

        [ParamsSource(nameof(NumObjectsConfig))]
        public int NumObjects { get; set; }


        private VideoService _videoService = new();
        private VideoFlatBuffersConverter _videoconverter = new();
        private Video vid;
        private byte[] vidSerialized;


        [GlobalSetup]
        public void GlobalSetup()
        {
            vid = _videoService.CreateVideo();
            vidSerialized = _videoconverter.Serialize(vid);
        }

        [Benchmark]
        public void FullProcess() => _videoconverter.Deserialize(_videoconverter.Serialize(vid));

        [Benchmark]
        public void Serialization() => _videoconverter.Serialize(vid);

        [Benchmark]
        //[Arguments(1_000)]
        //[Arguments(100_000)]
        //[Arguments(1_000_000)]
        public void Serialization_Multiple()
        {
            for (int i = 0; i <= NumObjects; i++) _videoconverter.Serialize(vid);
        }

        //[Benchmark]
        //public void Serialization_1_000_000()
        //{
        //    for (int i = 0; i <= 1_000_000; i++) _videoconverter.Serialize(vid);
        //}

        [Benchmark]
        public void DeSerialization() => _videoconverter.Deserialize(vidSerialized);


        private class AntiVirusFriendlyConfig : ManualConfig
        {
            public AntiVirusFriendlyConfig()
            {
                //AddJob(Job.MediumRun.WithMinIterationCount(10).WithMaxIterationCount(1_000_000).WithToolchain(InProcessNoEmitToolchain.Instance));
                AddJob(Job.MediumRun.WithToolchain(InProcessNoEmitToolchain.Instance));

                AddAnalyser(EnvironmentAnalyser.Default);
            }

            //public AntiVirusFriendlyConfig()
            //{
            //    AddJob(Job.MediumRun
            //        .WithToolchain(InProcessNoEmitToolchain.Instance));
            //}
        }
    }
}
