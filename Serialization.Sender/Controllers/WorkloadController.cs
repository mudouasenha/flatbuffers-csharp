using Microsoft.AspNetCore.Mvc;
using Serialization.Services;
using Serialization.Services.Extensions;

namespace Serializaion.Sender.Controllers
{
#pragma warning disable CS4014

    [Route("sender/workload")]
    public class WorkloadController : ControllerBase
    {
        private readonly WorkloadService _workloadService = new();

        [HttpPost]
        public IActionResult Workload([FromQuery] string serializerType, [FromQuery] string serializationType, [FromQuery] int numThreads = 10)
        {
            try
            {
                var serializer = serializerType.GetSerializer();
                var targetType = serializationType.GetTargetType();

                _workloadService.RunParallelRestAsync(serializer, targetType, numThreads);

                return Ok($"Parallel Processing Async Service for {nameof(Workload)} initiated");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return Problem();
            }
        }
    }
}