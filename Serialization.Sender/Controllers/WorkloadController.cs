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
        private readonly WorkloadService _workloadService;

        public WorkloadController(ILogger<WorkloadController> logger, WorkloadService senderService) => (_logger, _workloadService) = (logger, senderService);

        [HttpPost("FlatBuffers")]
        public IActionResult FlatBuffers([FromQuery] int numThreads = 10, [FromQuery] int numMessages = 10, [FromQuery] bool rest = false)
        {
            try
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                if (rest)
                {
                    _workloadService.RunParallelRestAsync(new FlatBuffersSerializer(), numThreads, numMessages);
                }
                else
                {
                    _workloadService.RunParallelAsync(new FlatBuffersSerializer(), numThreads, numMessages);
                }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return Ok($"Parallel Processing Async Service for {nameof(FlatBuffers)} initiated. TYPE = REST? {rest}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }

        [HttpPost("MessagePack")]
        public IActionResult MessagePack([FromQuery] int numThreads = 10, [FromQuery] int numMessages = 10, [FromQuery] bool rest = false)
        {
            try
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                if (rest)
                {
                    _workloadService.RunParallelRestAsync(new MessagePackCSharpSerializer(), numThreads, numMessages);
                }
                else
                {
                    _workloadService.RunParallelAsync(new MessagePackCSharpSerializer(), numThreads, numMessages);
                }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return Ok($"Parallel Processing Async Service for {nameof(MessagePack)} initiated. TYPE = REST? {rest}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }

        [HttpPost("systemtextjson")]
        public IActionResult SystemTextJson([FromQuery] int numThreads = 10, [FromQuery] int numMessages = 10, [FromQuery] bool rest = false)
        {
            try
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                if (rest)
                {
                    _workloadService.RunParallelRestAsync(new NewtonsoftJsonSerializer(), numThreads, numMessages);
                }
                else
                {
                    _workloadService.RunParallelAsync(new NewtonsoftJsonSerializer(), numThreads, numMessages);
                }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return Ok($"Workload Processing Service for {nameof(SystemTextJson)} initiated. TYPE = REST? {rest}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }
    }
}