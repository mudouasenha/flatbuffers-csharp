using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Mvc;
using Serialization.Benchmarks;

namespace Serializaion.Sender.Controllers
{
    [ApiController]
    [Route("sender/[controller]")]
    public class BenchmarkController : ControllerBase
    {
        private readonly ILogger<BenchmarkController> _logger;
        private readonly FlatBuffersBenchmark _flatBuffersBenchmark;

        public BenchmarkController(ILogger<BenchmarkController> logger, FlatBuffersBenchmark senderService) => (_logger, _flatBuffersBenchmark) = (logger, senderService);

        [HttpPost("flatbuffers")]
        public IActionResult Benchmark()
        {
            try
            {
                var summary = BenchmarkRunner.Run<FlatBuffersBenchmark>();
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