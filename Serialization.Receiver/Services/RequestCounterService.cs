using Serialization.Services.CsvExporter;

namespace Serialization.Receiver.Services
{
    public class RequestCounterService : IRequestCounterService
    {
        private static readonly RequestCounterService _instance;
        private static readonly string Serialize = "Serialize";
        private static readonly string Deserialize = "Deserialize";
        private static readonly string SerializeAndDeserialize = "SerializeAndDeserialize";
        private string FileName = "Measurements-REST-Server-SerializationBenchmark";
        private readonly object _lock = new();
        private int counterSerialize = 0;
        private int counterDeserialize = 0;
        private int counterSerializeAndDeserialize = 0;
        private short serializerType;
        private Timer timer;
        private ExecutionInfo executionInfo;
        private readonly List<MeasurementRestReceiver> countsPerSecond = new();

        public RequestCounterService()
        {
            timer = new Timer(RecordCountsPerSecond, null, Timeout.Infinite, Timeout.Infinite);
            IncrementCounter(0);
        }

        public void IncrementCounter(short serializer, bool? serialize = null)
        {
            lock (_lock)
            {
                if (serialize == null) counterSerializeAndDeserialize++;
                else if (serialize == true) counterSerialize++;
                else if (serialize == false) counterDeserialize++;

                serializerType = serializer;
            }
        }

        private void RecordCountsPerSecond(object state)
        {
            lock (_lock)
            {
                var datetime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerialize, datetime, Serialize, 0));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterDeserialize, datetime, Deserialize, 0));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerializeAndDeserialize, datetime, SerializeAndDeserialize, 0));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerialize, datetime, Serialize, 1));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterDeserialize, datetime, Deserialize, 1));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerializeAndDeserialize, datetime, SerializeAndDeserialize, 1));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerialize, datetime, Serialize, 2));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterDeserialize, datetime, Deserialize, 2));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerializeAndDeserialize, datetime, SerializeAndDeserialize, 2));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerialize, datetime, Serialize, 3));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterDeserialize, datetime, Deserialize, 3));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerializeAndDeserialize, datetime, SerializeAndDeserialize, 3));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerialize, datetime, Serialize, 4));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterDeserialize, datetime, Deserialize, 4));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerializeAndDeserialize, datetime, SerializeAndDeserialize, 4));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerialize, datetime, Serialize, 5));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterDeserialize, datetime, Deserialize, 5));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerializeAndDeserialize, datetime, SerializeAndDeserialize, 5));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerialize, datetime, Serialize, 6));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterDeserialize, datetime, Deserialize, 6));
                //countsPerSecond.Add(new MeasurementRestReceiver(counterSerializeAndDeserialize, datetime, SerializeAndDeserialize, 6));

                countsPerSecond.Add(new MeasurementRestReceiver(counterSerialize, datetime, Serialize, serializerType));
                countsPerSecond.Add(new MeasurementRestReceiver(counterDeserialize, datetime, Deserialize, serializerType));
                countsPerSecond.Add(new MeasurementRestReceiver(counterSerializeAndDeserialize, datetime, SerializeAndDeserialize, serializerType));
                //Console.WriteLine(measurement.ToString());
                counterSerialize = 0;
                counterDeserialize = 0;
                counterSerializeAndDeserialize = 0;
            }
        }

        public void StartMonitoring()
        {
            timer.Change(1000, 1000);
            //SaveToCsv(this);
        }

        public void SaveToCsv(string serializerType, string serializationType, int numThreads, string method)
        {
            executionInfo = new ExecutionInfo(serializationType, serializerType, 0, numThreads, method);
            SaveToCsv(this);
        }

        private void SaveToCsv(object state)
        {
            lock (_lock)
            {
                var serializer = SerializersDictionary.GetStringFromKey(serializerType);
                CsvExporter.ExportMeasurementsRESTReceiver($"{FileName}-{serializer}.csv", countsPerSecond, executionInfo);
                countsPerSecond.Clear();
                timer.Change(Timeout.Infinite, Timeout.Infinite);
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