using BenchmarkDotNet.Attributes;
using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Abstractions;
using Google.FlatBuffers;

namespace FlatBuffers.Sender
{
    public class SenderService : IBenchMarkService<Video>
    {
        private readonly ILogger<SenderService> _logger;
        private readonly IVideoService _videoService;
        private readonly ReceiverClient _receiverClient;
        private readonly IVideoSerializationService _videoSerializationService;

        public SenderService(ILogger<SenderService> logger, IVideoSerializationService videoSerializationService, IVideoService videoService)
        {
            _logger = logger;
            _videoSerializationService = videoSerializationService;
            _videoService = videoService;
        }

        [Benchmark]
        public async Task<Video> RunBenchMark()
        {
            var vid = _videoService.CreateVideo();
            var str = _videoSerializationService.Serialize(vid);
            //t1
            var byteArr = str.ToArray(str.Position, str.Length);

            var resp = await _receiverClient.PostAsync("/video", byteArr);

            var ms = (MemoryStream)resp;
            byte[] byteArrResp = ms.ToArray();

            var videoSer = _videoSerializationService.Deserialize(new ByteBuffer(byteArrResp));

            var video = new Video().FromSerializationModel(videoSer);

            return video;
        }
    }
}