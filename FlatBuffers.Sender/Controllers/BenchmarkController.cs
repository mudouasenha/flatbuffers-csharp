using BenchmarkDotNet.Running;
using FlatBuffers.Domain.Entities;
using FlatBuffers.Domain.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FlatBuffers.Sender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BenchmarkController : ControllerBase
    {
        private readonly ILogger<BenchmarkController> _logger;
        private readonly IVideoService _videoService;
        private readonly ReceiverClient _receiverClient;
        private readonly IVideoSerializationService _videoSerializationService;

        public BenchmarkController(ILogger<BenchmarkController> logger, IVideoSerializationService videoSerializationService, IVideoService videoService)
        {
            _logger = logger;
            _videoSerializationService = videoSerializationService;
            _videoService = videoService;
        }

        [HttpPost]
        public IActionResult Benchmark()
        {
            try
            {
                var summary = BenchmarkRunner.Run<IBenchMarkService<VideoEntity>>();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }
    }
}