using Microsoft.AspNetCore.Mvc;
using Serialization.Domain;
using Serialization.Domain.Entities;
using Serialization.Domain.Interfaces;
using Serialization.Receiver.Services;
using Serialization.Serializers.ApacheAvro;
using Serialization.Serializers.CapnProto;
using Serialization.Services;
using Serialization.Services.Extensions;
using Serialization.Services.Models;

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

        [HttpPost("deserialize")]
        [Consumes("application/octet-stream")]
        [Produces("application/octet-stream")]
        public async Task Deserialize([FromQuery] string serializerType, [FromQuery] string serializationType, [FromBody] byte[] requestData)
        {
            await Task.Run(() =>
            {
                (var serializer, short key) = serializerType.GetSerializer();
                var targetType = serializationType.GetTargetType();

                var deserializationType = serializer.BenchmarkDeserialize(targetType, requestData);
                requestCounterService.IncrementCounter(key, false);
            });
        }

        [HttpPost("serialize/video")]
        [Produces("application/octet-stream")]
        public async Task<FileContentResult> SerializeVideo([FromQuery] string serializerType, [FromQuery] string serializationType, [FromBody] Video serializableObject)
        {
            (var serializer, short key) = serializerType.GetSerializer();

            var targetType = serializationType.GetTargetType();

            return Serialize(serializer, key, serializableObject, targetType);
        }

        [HttpPost("serialize/videoinfo")]
        [Produces("application/octet-stream")]
        public async Task<FileContentResult> SerializeVideoInfo([FromQuery] string serializerType, [FromQuery] string serializationType, [FromBody] VideoInfo serializableObject)
        {
            (var serializer, short key) = serializerType.GetSerializer();
            requestCounterService.IncrementCounter(key, true);
            var targetType = serializationType.GetTargetType();

            return Serialize(serializer, key, serializableObject, targetType);
        }

        [HttpPost("serialize/channel")]
        [Produces("application/octet-stream")]
        public async Task<FileContentResult> SerializeChannel([FromQuery] string serializerType, [FromQuery] string serializationType, [FromBody] Channel serializableObject)
        {
            (var serializer, short key) = serializerType.GetSerializer();
            requestCounterService.IncrementCounter(key, true);
            var targetType = serializationType.GetTargetType();

            return Serialize(serializer, key, serializableObject, targetType);
        }

        [HttpPost("serialize/socialinfo")]
        [Produces("application/octet-stream")]
        public async Task<FileContentResult> SerializeSocialInfo([FromQuery] string serializerType, [FromQuery] string serializationType, [FromBody] SocialInfo serializableObject)
        {
            (var serializer, short key) = serializerType.GetSerializer();
            requestCounterService.IncrementCounter(key, true);
            var targetType = serializationType.GetTargetType();

            return Serialize(serializer, key, serializableObject, targetType);
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

        [HttpPost("generate-requests")]
        public List<RequestMessage> GenerateRequests([FromQuery] string serializerType, [FromQuery] int numMessages)
        {
            var myService = new WorkloadService();

            List<RequestMessage> messages = myService.GenerateRequests(serializerType, numMessages);

            return messages;
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

        private FileContentResult Serialize(ISerializer serializer, short key, ISerializationTarget serializable, Type targetType)
        {
            long size;
            object serialized;

            GenerateIntermediateIfNeeded(serializable, serializer);

            size = serializable.Serialize(serializer);

            serializer.GetSerializationResult(targetType, out serialized);

            Response.ContentType = "application/octet-stream";
            Response.Headers.Add("Content-Length", size.ToString());

            requestCounterService.IncrementCounter(key, true);
            return File((byte[])serialized, "application/octet-stream");
        }
    }
}