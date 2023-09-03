using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Mvc;
using Serialization.Benchmarks;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.SystemTextJson;
using Serialization.Services;

namespace Serializaion.Sender.Controllers
{
    [ApiController]
    [Route("sender/[controller]")]
    public class WorkloadController : ControllerBase
    {
        private readonly ILogger<WorkloadController> _logger;
        private readonly SenderService _senderService;

        public WorkloadController(ILogger<WorkloadController> logger, SenderService senderService) => (_logger, _senderService) = (logger, senderService);

        [HttpPost("flatbuffers")]
        public IActionResult FlatBuffers([FromQuery] int numThreads = 10)
        {
            try
            {
                _senderService.RunParallelProcessingAsync(numThreads, new VideoFlatBuffersSerializer());
                return Ok("Service initiated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }

        [HttpPost("system-text-json")]
        public IActionResult SystemTextJson([FromQuery] int numThreads = 10)
        {
            try
            {
                _senderService.RunParallelProcessingAsync(numThreads, new SytemTextJsonSerializer());
                return Ok("Service initiated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }
    }
}