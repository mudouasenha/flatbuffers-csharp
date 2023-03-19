using BenchmarkDotNet.Attributes;
using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Abstractions;
using FlatBuffers.Domain.Services.Converters.Abstractions;

namespace FlatBuffers.Sender
{
    public class SenderService : IBenchMarkService<Video>
    {
        private readonly ILogger<SenderService> _logger;
        private readonly IVideoService _videoService;
        private readonly ReceiverClient _receiverClient;
        private readonly IFlatBuffersVideoConverter _videoconverter;

        public SenderService(ILogger<SenderService> logger, IFlatBuffersVideoConverter videoConverter, IVideoService videoService, ReceiverClient receiverClient)
        {
            _logger = logger;
            _videoconverter = videoConverter;
            _videoService = videoService;
            _receiverClient = receiverClient;
        }

        [Benchmark]
        public async Task<Video> RunBenchMark()
        {
            try
            {
                var vid = _videoService.CreateVideo();
                var byteArr = _videoconverter.Serialize(vid);

                var resp = await _receiverClient.PostAsync("/video", byteArr);

                var ms = (MemoryStream)resp;
                byte[] byteArrResp = ms.ToArray();

                var video = _videoconverter.Deserialize(byteArrResp);

                return video;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                throw;
            }
        }
    }
}