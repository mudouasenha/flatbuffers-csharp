using Microsoft.AspNetCore.Mvc.Formatters;

namespace FlatBuffers.Domain.Formatters
{
    /*public class ByteArrayInputFormatter : MediaTypeFormatter
    {
        public ByteArrayInputFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(byte[]);
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var stream = new MemoryStream();
            await content.CopyToAsync(stream);
            return stream.ToArray();
        }

        public virtual async Task WriteToStream(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            await content.CopyToAsync(writeStream);
            return InputFormatterResult.SuccessAsync(writeStream.ToArray());
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(byte[]);
        }
    }*/

    public class ByteArrayInputFormatter : IInputFormatter
    {
        private const string ContentType = "application/octet-stream";

        public bool CanRead(InputFormatterContext context) =>
            context.HttpContext.Request.ContentType == ContentType;

        public async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            var stream = new MemoryStream();
            await request.Body.CopyToAsync(stream);
            return await InputFormatterResult.SuccessAsync(stream.ToArray());
        }
    }
}