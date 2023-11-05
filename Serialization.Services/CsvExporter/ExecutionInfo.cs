using System.Diagnostics;

namespace Serialization.Services.CsvExporter
{
    public static class SerializersDictionary
    {
        public static Dictionary<short, string> Serializers { get; set; } = new Dictionary<short, string>()
        {
            { 0, "Avro" },
            { 1, "CapnProto" },
            { 2, "FlatBuffers" },
            { 3, "MessagePack-CSharp" },
            { 4, "Newtonnsoft.Json" },
            { 5, "Protobuf" },
            { 6, "Thrift" },
            { 7, "BinaryFormatter" },
        };

        public static string GetStringFromKey(short key)
        {
            if (Serializers.TryGetValue(key, out string value))
            {
                return value;
            }
            return "Key not found";
        }
    }

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
        public MeasurementRestReceiver(int requests, long timestamp, string method, short serializerKey)
        {
            Timestamp = timestamp;
            Requests = requests;
            Method = method;
            Serializer = SerializersDictionary.GetStringFromKey(serializerKey);
        }
        public string Serializer { get; set; }
        public string Method { get; set; }
        public long Timestamp { get; set; }
        public int Requests { get; set; }

        public override string ToString() => $"Timestamp: {Timestamp};    Requests/s: {Requests}";
    }
}