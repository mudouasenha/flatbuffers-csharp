
using BenchmarkDotNet.Attributes;
using Serialization.Domain.Entities;
using Serialization.Serializers.FlatBuffers;
using Serialization.Services;

namespace Serialization.Benchmarks
{
    [HtmlExporter]
    [RPlotExporter]
    [RyuJitX64Job]
    public class FlatBuffersBenchmark
    {
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
        public Video RunBenchmarkFullProcess()
        {
            var byteArr = _videoconverter.Serialize(vid);
            return _videoconverter.Deserialize(byteArr);
        }

        [Benchmark]
        public int RunBenchmarkFullProcesstest()
        {
            return 1;
        }

        [Benchmark]
        public byte[] RunBenchmarkSerialization() => _videoconverter.Serialize(vid);

        //[Benchmark]
        //public void RunBenchmarkSerialization2()
        //{
        //    for (int i = 0; i <= 1000000; i++) _videoconverter.Serialize(vid);
        //}

        [Benchmark]
        public Video RunBenchmarkDeSerialization() => _videoconverter.Deserialize(vidSerialized);
    }
}
