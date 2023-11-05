using Microsoft.AspNetCore.Mvc;
using Serialization.Services;
using Serialization.Services.Extensions;

namespace Serializaion.Sender.Controllers
{
#pragma warning disable CS4014

    [Route("sender/workload")]
    public class WorkloadController : ControllerBase
    {
        private readonly WorkloadService _workloadService;

        public WorkloadController(WorkloadService workloadService)
        {
            _workloadService = workloadService;
        }

        [HttpPost]
        public IActionResult Workload([FromQuery] string serializerType, [FromQuery] string serializationType, [FromQuery] int numThreads = 10)
        {
            try
            {
                (var serializer, short _) = serializerType.GetSerializer();
                var targetType = !string.IsNullOrEmpty(serializationType) ? serializationType.GetTargetType() : null;

                _workloadService.RunParallelRestAsync(serializer, targetType, numThreads);

                return Ok($"Parallel Processing Async Service for {nameof(Workload)} initiated");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return Problem();
            }
        }

        [HttpPost("clear")]
        public IActionResult Clear()
        {
            try
            {
                _workloadService.Clear();

                return Ok($"Parallel Processing Cleared");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return Problem();
            }
        }
    }
}