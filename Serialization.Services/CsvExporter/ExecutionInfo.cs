using System.Diagnostics;

namespace Serialization.Services.CsvExporter
{
    public class ExecutionInfo
    {
        public ExecutionInfo(string target, string serializer, int numMessages, int numThreads)
        {
            Target = target;
            Serializer = serializer;
            NumMessages = numMessages;
            NumThreads = numThreads;
        }

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
        public MeasurementRest(long ticks)
        {
            Latency = ticks / Stopwatch.Frequency * 1000000000;
            Timestamp = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
        }

        public double Latency { get; set; }
        public long Timestamp { get; set; }
    }

    public record MeasurementRestReceiver
    {
        public MeasurementRestReceiver(int requests)
        {
            Timestamp = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
            Requests = requests;
        }

        public double Timestamp { get; set; }
        public int Requests { get; set; }

        public override string ToString() => $"Timestamp: {Timestamp};    Requests/s: {Requests}";
    }
}