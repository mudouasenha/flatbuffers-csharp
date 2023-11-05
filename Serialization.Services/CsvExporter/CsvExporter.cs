namespace Serialization.Services.CsvExporter
{
    public static class CsvExporter
    {
        public static void ExportMeasurementsREST(string filePath, ExecutionInfo info)
        {
            using var writer = new StreamWriter(filePath, true);
            if (!File.Exists(filePath)) writer.WriteLine("Target,Method,Serializer,NumMessages,NumThreads,Timestamp,Latency");

            foreach (var measurement in info.Measurements)
            {
                var line = $"{EscapeCsvField(info.Target)},{EscapeCsvField(measurement.Method)},{EscapeCsvField(info.Serializer)},{info.NumMessages},{info.NumThreads},{measurement.Timestamp},{measurement.Latency}";
                writer.WriteLine(line);
            }
        }

        public static void ExportMeasurementsRESTReceiver(string filePath, IReadOnlyCollection<MeasurementRestReceiver> measurements, ExecutionInfo info)
        {
            if (!File.Exists(filePath))
            {
                using var writer = new StreamWriter(filePath, false);
                writer.WriteLine("Target,Serializer,Method,NumMessages,NumThreads,Timestamp,Requests");
            }

            using (var writer = new StreamWriter(filePath, true))
            {
                foreach (var measurement in measurements)
                {
                    var line = $"{EscapeCsvField(info.Target)},{EscapeCsvField(measurement.Serializer)},{EscapeCsvField(measurement.Method)},{info.NumMessages},{info.NumThreads},{measurement.Timestamp},{measurement.Requests}";
                    writer.WriteLine(line);
                }
            }
        }

        private static string EscapeCsvField(string field) => !string.IsNullOrEmpty(field) && field.Contains(',') ? $"\"{field}\"" : field;
    }
}