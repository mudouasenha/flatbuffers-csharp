using System.Diagnostics;

namespace Serialization.Benchmarks.Benchmarks
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

        public List<Measurement> Measurements { get; set; } = new List<Measurement>();
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

    public class CsvExporter
    {
        private List<ExecutionInfo> executionInfos = new List<ExecutionInfo>();

        public void AddExecutionInfo(ExecutionInfo executionInfo) => executionInfos.Add(executionInfo);

        public void ExportToCsv(string filePath)
        {
            using (var writer = new StreamWriter(filePath, true)) // Append mode
            {
                if (!File.Exists(filePath))
                {
                    // If the file doesn't exist, write the CSV header
                    writer.WriteLine("Target,Serializer,NumMessages,NumThreads,ExecutionTime,MessageSize");
                }

                foreach (var execution in executionInfos)
                {
                    foreach (var measurement in execution.Measurements)
                    {
                        var line = $"{EscapeCsvField(execution.Target)},{EscapeCsvField(execution.Serializer)},{execution.NumMessages},{execution.NumThreads},{measurement.Nanoseconds},{measurement.SerializationSize}";
                        writer.WriteLine(line);
                    }
                }
            }
        }

        private string EscapeCsvField(string field)
        {
            // Escape field if it contains a comma
            if (field.Contains(","))
            {
                return $"\"{field}\"";
            }
            return field;
        }
    }
}
