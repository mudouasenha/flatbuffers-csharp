using Microsoft.AspNetCore.Mvc;
using Serialization.Domain;
using Serialization.Domain.Interfaces;
using Serialization.Receiver.Services;
using Serialization.Serializers.ApacheAvro;
using Serialization.Services.Extensions;

namespace Serialization.Receiver.Controllers
{
    [ApiController]
    [Route("receiver/serializer")]
    public class SerializerController : ControllerBase
    {
        private readonly RequestCounterService requestCounterService;

        public SerializerController(RequestCounterService requestCounterService)
        {
            this.requestCounterService = requestCounterService;
        }

        [HttpPost()]
        [Consumes("application/octet-stream")]
        [Produces("application/octet-stream")]
        public async Task<FileContentResult> DeserializeAndSerialize([FromQuery] string serializerType, [FromQuery] string serializationType, [FromBody] byte[] requestData)
        {
            try
            {
                var serializer = serializerType.GetSerializer();
                var targetType = serializationType.GetTargetType();

                var deserializationType = serializer.BenchmarkDeserialize(targetType, requestData);
                long size;

                serializer.GetDeserializationResult(targetType, out ISerializationTarget serializableObject);

                GenerateIntermediateIfNeeded(serializableObject, serializer);

                size = serializableObject.Serialize(serializer);

                if (serializer.GetSerializationResult(targetType, out var serialized))
                {
                    Response.ContentType = "application/octet-stream";
                    Response.Headers.Add("Content-Length", size.ToString());

                    requestCounterService.IncrementCounter();
                    return File((byte[])serialized, "application/octet-stream");
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        [HttpPost("deserialize")]
        [Consumes("application/octet-stream")]
        [Produces("application/octet-stream")]
        public async Task Deserialize([FromQuery] string serializerType, [FromQuery] string serializationType, [FromBody] byte[] requestData)
        {
            try
            {
                var serializer = serializerType.GetSerializer();
                var targetType = serializationType.GetTargetType();

                var deserializationType = serializer.BenchmarkDeserialize(targetType, requestData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [HttpPost()]
        [Consumes("application/octet-stream")]
        [Produces("application/octet-stream")]
        public async Task<FileContentResult> Serialize([FromQuery] string serializerType, [FromQuery] string serializationType, [FromBody] ISerializationTarget serializableObject)
        {
            try
            {
                var serializer = serializerType.GetSerializer();
                var targetType = serializationType.GetTargetType();

                long size;

                GenerateIntermediateIfNeeded(serializableObject, serializer);

                size = serializableObject.Serialize(serializer);

                if (serializer.GetSerializationResult(targetType, out var serialized))
                {
                    Response.ContentType = "application/octet-stream";
                    Response.Headers.Add("Content-Length", size.ToString());

                    requestCounterService.IncrementCounter();
                    return File((byte[])serialized, "application/octet-stream");
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        [HttpPost("monitoring/start")]
        public async Task<IActionResult> StartMonitoring([FromQuery] string serializerType)
        {
            requestCounterService.StartMonitoring();
            return Ok();
        }

        [HttpPost("monitoring/save-results")]
        public async Task<IActionResult> RecordCsv([FromQuery] string serializerType)
        {
            requestCounterService.SaveToCsv();
            return Ok();
        }

        private void GenerateIntermediateIfNeeded(ISerializationTarget obj, ISerializer serializer)
        {
            if (serializer is ProtobufSerializer)
                obj.CreateProtobufMessage();
            if (serializer is ApacheThriftSerializer)
                obj.CreateThriftMessage();
            if (serializer is ApacheAvroSerializer)
                obj.CreateAvroMessage();
        }
    }
}