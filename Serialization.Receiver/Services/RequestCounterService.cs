using Serialization.Services.CsvExporter;

namespace Serialization.Receiver.Services
{
    public class RequestCounterService : IRequestCounterService
    {
        private readonly CsvExporter _csvExporter = new();
        private static RequestCounterService _instance;
        private readonly object _lock = new();
        private int counter = 0;
        private string ParamsString;
        private readonly Timer timer;
        private readonly Timer csvTimer;
        private ExecutionInfo executionInfo;
        private List<MeasurementRestReceiver> countsPerSecond = new();

        public RequestCounterService()
        {
            timer = new Timer(RecordCountsPerSecond, null, 1000, 1000);
            // csvTimer = new Timer(SaveToCsv, null, 10000, 10000);
        }

        //public static RequestCounterService Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (_instance)
        //            {
        //                if (_instance == null)
        //                {
        //                    _instance = new RequestCounterService();
        //                }
        //            }
        //        }
        //        return _instance;
        //    }
        //}

        public void IncrementCounter()
        {
            lock (_lock)
            {
                counter++;
            }
        }

        private void RecordCountsPerSecond(object state)
        {
            lock (_lock)
            {
                var measurement = new MeasurementRestReceiver(counter, DateTimeOffset.Now.ToUnixTimeMilliseconds());
                countsPerSecond.Add(measurement);
                //Console.WriteLine(measurement.ToString());
                counter = 0;
            }
        }

        public void StartMonitoring()
        {
            SaveToCsv(this);
        }

        public void SaveToCsv(string datetime, string serializerType, string serializationType, int numThreads, string method)
        {
            ParamsString = $"{datetime}";
            executionInfo = new ExecutionInfo(serializationType, serializerType, 0, numThreads, method);
            SaveToCsv(this);
        }

        private void SaveToCsv(object state)
        {
            lock (_lock)
            {
                _csvExporter.ExportMeasurementsRESTReceiver($"Measurements-REST-Server-SerializationBenchmark.csv", countsPerSecond, executionInfo);
                countsPerSecond.Clear();
            }
        }

        public IReadOnlyList<MeasurementRestReceiver> GetCountsPerSecond()
        {
            lock (_lock)
            {
                return countsPerSecond.AsReadOnly();
            }
        }
    }
}