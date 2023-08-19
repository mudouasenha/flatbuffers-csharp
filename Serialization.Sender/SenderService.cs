using BenchmarkDotNet.Attributes;
using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services;
using FlatBuffers.Domain.Services.Flatbuffers;

namespace FlatBuffers.Sender
{
    [HtmlExporter]
    [RPlotExporter]
    public class SenderService
    {
        private static VideoService _videoService;
        private static VideoFlatBuffersConverter _videoconverter;
        private static Video vid;
        private static byte[] vidSerialized;


        [GlobalSetup]
        public void GlobalSetup()
        {
            _videoconverter = new VideoFlatBuffersConverter();
            _videoService = new VideoService();
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
        //public void RunBenchmarkSerialization2() {
        //    for (int i = 0; i <= 1000000; i++) _videoconverter.Serialize(vid);
        //}

        [Benchmark]
        public Video RunBenchmarkDeSerialization() => _videoconverter.Deserialize(vidSerialized);
    }
}