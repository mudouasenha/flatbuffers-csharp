using Microsoft.AspNetCore.Mvc;
using Serialization.Serializers.FlatBuffers;
using Serialization.Serializers.MessagePack;
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
        public IActionResult FlatBuffers([FromQuery] int numThreads = 10, [FromQuery] int numMessages = 10)
        {
            try
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _senderService.RunParallelProcessingAsync(new FlatBuffersSerializer(), numThreads, numMessages);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return Ok($"Parallel Processing Async Service for {nameof(FlatBuffers)} initiated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }

        [HttpPost("mesagepack")]
        public IActionResult MessagePack([FromQuery] int numThreads = 10, [FromQuery] int numMessages = 10)
        {
            try
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _senderService.RunParallelProcessingAsync(new MessagePackCSharpSerializer(), numThreads, numMessages);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return Ok($"Parallel Processing Async Service for {nameof(MessagePack)} initiated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }

        [HttpPost("system-text-json")]
        public IActionResult SystemTextJson([FromQuery] int numThreads = 10, [FromQuery] int numMessages = 10)
        {
            try
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _senderService.RunParallelProcessingAsync(new SytemTextJsonSerializer(), numThreads, numMessages);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return Ok($"Parallel Processing Async Service for {nameof(SystemTextJson)} initiated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }
    }
}