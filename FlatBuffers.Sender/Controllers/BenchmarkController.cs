using BenchmarkDotNet.Running;
using FlatBuffers.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FlatBuffers.Sender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BenchmarkController : ControllerBase
    {
        private readonly ILogger<BenchmarkController> _logger;
        private readonly IBenchMarkService<Video> _senderService;

        public BenchmarkController(ILogger<BenchmarkController> logger, IBenchMarkService<Video> senderService) => (_logger, _senderService) = (logger, senderService);

        [HttpPost]
        public IActionResult Benchmark()
        {
            try
            {
                var summary = BenchmarkRunner.Run<IBenchMarkService<Video>>();
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
                var result = await _senderService.RunBenchMark();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem(ex.Message);
            }
        }
    }
}