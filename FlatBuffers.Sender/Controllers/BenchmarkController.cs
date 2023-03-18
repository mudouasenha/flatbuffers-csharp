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

        public BenchmarkController(ILogger<BenchmarkController> logger) => _logger = logger;

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
    }
}