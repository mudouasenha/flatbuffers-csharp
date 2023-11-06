using Microsoft.AspNetCore.Mvc;
using Serialization.Domain;
using Serialization.Domain.Interfaces;
using Serialization.Receiver.Services;
using Serialization.Serializers.ApacheAvro;
using Serialization.Serializers.CapnProto;
using Serialization.Services.Extensions;
using System.Diagnostics;

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

        [HttpPost]
        [Consumes("application/octet-stream")]
        [Produces("application/octet-stream")]
        public FileContentResult DeserializeAndSerialize([FromQuery] string serializerType, [FromQuery] string serializationType, [FromBody] byte[] requestData)
        {

            (ISerializer serializer, short key) = serializerType.GetSerializer();
            requestCounterService.IncrementCounter(key);
            var targetType = serializationType.GetTargetType();

            var deserializationType = serializer.BenchmarkDeserialize(targetType, requestData);
            long size;
            object serialized;

            serializer.GetDeserializationResult(targetType, out ISerializationTarget serializableObject);

            GenerateIntermediateIfNeeded(serializableObject, serializer);

            size = serializableObject.Serialize(serializer);

            if (serializer.GetSerializationResult(targetType, out serialized))
            {
            }

            Response.ContentType = "application/octet-stream";
            Response.Headers.Add("Content-Length", size.ToString());

            //requestCounterService.IncrementCounter(key);
            return new FileContentResult((byte[])serialized, "application/octet-stream");
        }

        [HttpGet("test")]
        [Consumes("application/octet-stream")]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> Test()
        {
            await Task.Run(() =>
            {
                short serializerKey = (short)new Random().Next(8);
                requestCounterService.IncrementCounter(serializerKey);
                requestCounterService.IncrementCounter(serializerKey, true);
                requestCounterService.IncrementCounter(serializerKey, false);
                //Console.WriteLine($"here, {DateTime.Now}");
                return Ok();
            });

            return Ok();
        }

        [HttpPost("deserialize")]
        public async Task Deserialize([FromQuery] string serializerType, [FromQuery] string serializationType, [FromBody] byte[] requestData)
        {
            await Task.Run(() =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                (var serializer, short key) = serializerType.GetSerializer();
                requestCounterService.IncrementCounter(key, false);
                var targetType = serializationType.GetTargetType();

                var deserializationType = serializer.BenchmarkDeserialize(targetType, requestData);
                //requestCounterService.IncrementCounter(key, false);
                sw.Stop();
                Console.WriteLine(sw.Elapsed.TotalMilliseconds * 1000000);
            });
        }

        [HttpPost("serialize")]
        [Consumes("application/json")]
        [Produces("application/octet-stream")]
        public async Task<FileContentResult> Serialize([FromQuery] string serializerType, [FromQuery] string serializationType, [FromBody] object serializableObject)
        {

            (var serializer, short key) = serializerType.GetSerializer();
            requestCounterService.IncrementCounter(key, true);
            var targetType = serializationType.GetTargetType();

            long size;
            object serialized;
            ISerializationTarget serializable = (ISerializationTarget)serializableObject;

            GenerateIntermediateIfNeeded(serializable, serializer);

            size = serializable.Serialize(serializer);

            if (serializer.GetSerializationResult(targetType, out serialized))
            {
            }

            Response.ContentType = "application/octet-stream";
            Response.Headers.Add("Content-Length", size.ToString());

            //requestCounterService.IncrementCounter(key, true);
            return File((byte[])serialized, "application/octet-stream");

            return null;
        }

        [HttpPost("monitoring/start")]
        public async Task<IActionResult> StartMonitoring([FromQuery] string serializerType)
        {
            requestCounterService.StartMonitoring();
            return Ok();
        }

        [HttpPost("monitoring/save-results")]
        public async Task<IActionResult> RecordCsv([FromQuery] string serializerType, [FromQuery] string serializationType, [FromQuery] int numThreads, [FromQuery] string method)
        {
            requestCounterService.SaveToCsv(serializerType, serializationType, numThreads, method);
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
            if (serializer is CapnProtoSerializer)
                obj.CreateCapnProtoMessage();
        }
    }
}