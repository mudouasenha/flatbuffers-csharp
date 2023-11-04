using System.Diagnostics;

namespace Serialization.Services.CsvExporter
{
    public class ExecutionInfo
    {
        public ExecutionInfo(string target, string serializer, int numMessages, int numThreads, string method = "")
        {
            Target = target;
            Serializer = serializer;
            NumMessages = numMessages;
            NumThreads = numThreads;
            Method = method;
        }

        public string Method { get; set; }
        public string Target { get; set; }
        public string Serializer { get; set; }
        public int NumMessages { get; set; }
        public int NumThreads { get; set; }

        public List<MeasurementRest> Measurements { get; set; } = new List<MeasurementRest>();
    }

    public class Measurement
    {
        public Measurement(long ticks, long serializationSize)
        {
            Nanoseconds = ticks * 1_000_000_000 / Stopwatch.Frequency;
            SerializationSize = serializationSize;
        }

        public long Nanoseconds { get; set; }
        public long SerializationSize { get; set; }
    }

    public record MeasurementRest
    {
        public MeasurementRest(long latency, long timestamp, string method)
        {
            Method = method;
            Latency = latency;
            Timestamp = timestamp;
        }
        public string Method { get; set; }
        public long Latency { get; set; }
        public long Timestamp { get; set; }
    }

    public record MeasurementRestReceiver
    {
        public MeasurementRestReceiver(int requests, long timestamp)
        {
            Timestamp = timestamp;
            Requests = requests;
        }
        public long Timestamp { get; set; }
        public int Requests { get; set; }

        public override string ToString() => $"Timestamp: {Timestamp};    Requests/s: {Requests}";
    }
}