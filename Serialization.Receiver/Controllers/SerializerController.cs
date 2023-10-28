using Microsoft.AspNetCore.Mvc;
using Serialization.Domain.Interfaces;
using Serialization.Services.Extensions;

namespace Serialization.Receiver.Controllers
{
    [ApiController]
    [Route("receiver/serializer")]
    public class SerializerController : ControllerBase
    {
        public SerializerController()
        {
        }

        [HttpPost()]
        public IActionResult DeserializeAndSerialize([FromQuery] string serializerType, [FromQuery] string originalType, [FromBody] object requestData)
        {
            try
            {
                var serializableObject = (ISerializationTarget)requestData;
                var serializer = serializerType.GetSerializer();
                var targetType = originalType.GetTargetType();

                serializer.BenchmarkDeserialize(targetType, serializableObject);
                var size = serializableObject.Serialize(serializer);

                if (serializer.GetSerializationResult(requestData.GetType(), out var result))
                {
                    Response.ContentType = "application/octet-stream";
                    Response.Headers.Add("Content-Length", size.ToString());

                    return Ok(new FileContentResult((byte[])result, "application/octet-stream"));
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return Problem();
            }
        }

        //[HttpPost()]
        //public IActionResult Test()
        //{
        //    try
        //    {

        //        return BadRequest();

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());

        //        return Problem();
        //    }
        //}
    }
}