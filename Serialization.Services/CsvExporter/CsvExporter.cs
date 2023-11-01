namespace Serialization.Services.CsvExporter
{
    public class CsvExporter
    {
        public void ExportMeasurementsREST(string filePath, ExecutionInfo info)
        {
            using var writer = new StreamWriter(filePath, true);
            if (!File.Exists(filePath)) writer.WriteLine("Target,Serializer,NumMessages,NumThreads,Timestamp,Latency");

            foreach (var measurement in info.Measurements)
            {
                var line = $"{EscapeCsvField(info.Target)},{EscapeCsvField(info.Serializer)},{info.NumMessages},{info.NumThreads},{measurement.Timestamp},{measurement.Latency}";
                writer.WriteLine(line);
            }
        }

        public void ExportMeasurementsRESTReceiver(string filePath, IReadOnlyCollection<MeasurementRestReceiver> measurements, ExecutionInfo info)
        {
            using var writer = new StreamWriter(filePath, true);
            if (!File.Exists(filePath)) writer.WriteLine("Target,Serializer,NumMessages,NumThreads,Timestamp,Requests");

            foreach (var measurement in measurements)
            {
                var line = $"{EscapeCsvField(info.Target)},{EscapeCsvField(info.Serializer)},{info.NumMessages},{info.NumThreads},{measurement.Timestamp},{measurement.Requests}";
                writer.WriteLine(line);
            }
        }

        public void ExportMeasurementsRESTReceiver(string filePath, IReadOnlyCollection<MeasurementRestReceiver> measurements)
        {
            using var writer = new StreamWriter(filePath, true);

            if (!File.Exists(filePath)) writer.WriteLine("Timestamp,Requests");

            foreach (var measurement in measurements)
            {
                var line = $"{measurement.Timestamp},{measurement.Requests}";
                writer.WriteLine(line);
            }
        }

        private string EscapeCsvField(string field) => field.Contains(',') ? $"\"{field}\"" : field;
    }
}