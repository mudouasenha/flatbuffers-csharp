namespace Serialization.Services.Models
{
    public class RequestMessage
    {
        public string Method { get; set; }

        public string Headers { get; set; }

        public string BinaryFilePath { get; set; }

        public long ByteSize { get; set; }

        public string Body { get; set; }

        public string Url { get; set; }

        public string ContentType { get; set; }
    }
}