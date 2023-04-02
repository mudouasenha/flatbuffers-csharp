using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;

namespace FlatBuffers.Domain.Formatters
{
    public class ByteArrayOutputFormatter : IOutputFormatter
    {
        private const string ContentType = "application/octet-stream";

        public bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            if (!context.ContentType.HasValue)
            {
                context.ContentType = new StringSegment(ContentType);
                return true;
            }

            return context.ContentType.Value == ContentType;
        }

        public Task WriteAsync(OutputFormatterWriteContext context)
        {
            context.HttpContext.Response.ContentType = ContentType;

            if (context.ObjectType == typeof(object))
            {
                if (context.Object == null)
                {
#if NETSTANDARD2_0
                    context.HttpContext.Response.Body.WriteByte(MessagePackCode.Nil);
                    return Task.CompletedTask;
#else
                    var writer = context.HttpContext.Response.BodyWriter;
                    var span = writer.GetSpan(1);
                    span[0] = MessagePackCode.Nil;
                    writer.Advance(1);
                    return writer.FlushAsync().AsTask();
#endif
                }
                else
                {
#if NETSTANDARD2_0
                    return MessagePackSerializer.SerializeAsync(context.Object.GetType(), context.HttpContext.Response.Body, context.Object, this.options, context.HttpContext.RequestAborted);
#else
                    var writer = context.HttpContext.Response.BodyWriter;
                    MessagePackSerializer.Serialize(context.Object.GetType(), writer, context.Object, this.options, context.HttpContext.RequestAborted);
                    return writer.FlushAsync().AsTask();
#endif
                }
            }
            else
            {
#if NETSTANDARD2_0
                return MessagePackSerializer.SerializeAsync(context.ObjectType, context.HttpContext.Response.Body, context.Object, this.options, context.HttpContext.RequestAborted);
#else
                var writer = context.HttpContext.Response.BodyWriter;
                MessagePackSerializer.Serialize(context.ObjectType, writer, context.Object, this.options, context.HttpContext.RequestAborted);
                return writer.FlushAsync().AsTask();
#endif
            }
        }
    }
}