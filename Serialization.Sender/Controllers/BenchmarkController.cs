using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Mvc;

namespace FlatBuffers.Sender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BenchmarkController : ControllerBase
    {
        private readonly ILogger<BenchmarkController> _logger;
        private readonly SenderService _senderService;

        public BenchmarkController(ILogger<BenchmarkController> logger, SenderService senderService) => (_logger, _senderService) = (logger, senderService);

        [HttpPost]
        public IActionResult Benchmark()
        {
            try
            {
                var summary = BenchmarkRunner.Run<SenderService>();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem();
            }
        }

        [HttpGet("video-result")]
        public async Task<IActionResult> Video()
        {
            try
            {
                //BenchmarkRunner.Run<SenderService>();
                // result = _senderService.RunBenchMarkLocal();
                //return Ok(result);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem(ex.Message);
            }
        }
    }
}