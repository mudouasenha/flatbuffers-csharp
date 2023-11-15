using Flurl;
using Serialization.Domain;
using Serialization.Domain.Builders;
using Serialization.Domain.Interfaces;
using Serialization.Serializers.ApacheAvro;
using Serialization.Serializers.CapnProto;
using Serialization.Services;
using Serialization.Services.CsvExporter;
using System.Diagnostics;
using System.Text;

namespace Serialization.Benchmarks
{
    public class RESTBenchmark
    {
        private RestClient client = new();
        private readonly HttpClient httpClient = new();
        private ExecutionInfo executionInfo;
        private static string BenchmarkDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        private const string fileName = "Measurements-REST-Client-SerializationBenchmark.csv";
        private readonly long epochTicks = new DateTime(1970, 1, 1).Ticks;
        private int seconds = 60;

        public ISerializer Serializer { get; set; }

        public ISerializationTarget Target { get; set; }

        public int[] NumsThreads { get; set; } = new int[] { 200, 500, 1000, 2000, 3000, 3500 };

        public int NumThreads { get; set; }

        public async Task Execute(ISerializer serializer)
        {
            Serializer = serializer;

            foreach (int n in NumsThreads)
            {
                NumThreads = n;
                executionInfo = new ExecutionInfo("Mixed", Serializer.ToString(), 0, NumThreads);

                Console.WriteLine($"Teste a executar: Serializer {Serializer}, Method {nameof(Serialize)}, NumThreads {NumThreads}");
                Console.ReadKey();
                Console.WriteLine("Teste inicializando.");
                await Serialize();
                CsvExporter.ExportMeasurementsREST(fileName, executionInfo);
                Serializer.Cleanup();
            }

            foreach (int n in NumsThreads)
            {
                NumThreads = n;
                executionInfo = new ExecutionInfo("Mixed", Serializer.ToString(), 0, NumThreads);
                Console.WriteLine($"Teste a executar: Serializer {Serializer}, Method {nameof(Deserialize)}, NumThreads {NumThreads}");
                Console.ReadKey();
                Console.WriteLine("Teste inicializando.");
                await Deserialize();

                CsvExporter.ExportMeasurementsREST(fileName, executionInfo);
                Serializer.Cleanup();
            }
        }

        private async Task Deserialize()
        {
            var majorStopwatch = new Stopwatch();
            majorStopwatch.Start();
            var measurement = 0;

            while (majorStopwatch.Elapsed.TotalSeconds < seconds)
            {
                IterationSetup();

                Serializer.GetSerializationResult(Target.GetType(), out var serializedTarget);
                var stopwatch = Stopwatch.StartNew();
                var response = client.PostAsync("receiver/serializer/deserialize"
                                .SetQueryParam("serializerType", Serializer.ToString())
                                .SetQueryParam("serializationType", Target.GetType().Name), serializedTarget).Result;
                stopwatch.Stop();
                var result = new MeasurementRest(stopwatch.ElapsedTicks, DateTimeOffset.Now.ToUnixTimeMilliseconds(), nameof(Deserialize));
                executionInfo.Measurements.Add(result);

                measurement++;
                Console.WriteLine($"Measurement {measurement}: {result.Latency} elapsed");
                // Wait for 1 second before the next iteration
                await Task.Delay(500);
            }

            majorStopwatch.Stop();
        }

        private async Task Serialize()
        {
            var majorStopwatch = new Stopwatch();
            majorStopwatch.Start();
            var measurement = 0;

            while (majorStopwatch.Elapsed.TotalSeconds < seconds)
            {
                IterationSetup();

                Serializer.GetSerializationResult(Target.GetType(), out var serializedTarget);
                var requestUrl = $"http://127.0.0.1:5020/receiver/serializer/serialize/{Target.ToString().ToLower()}"
                            .SetQueryParam("serializerType", Serializer.ToString())
                            .SetQueryParam("serializationType", Target.GetType().Name);

                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(Target);

                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                };

                var stopwatch = Stopwatch.StartNew();
                var response = httpClient.SendAsync(request).Result;

                stopwatch.Stop();

                var result = new MeasurementRest(stopwatch.ElapsedTicks, DateTimeOffset.Now.ToUnixTimeMilliseconds(), nameof(Serialize));

                executionInfo.Measurements.Add(result);

                measurement++;
                Console.WriteLine($"Measurement {measurement}: {result.Latency} elapsed");
                // Wait for 1 second before the next iteration
                await Task.Delay(500);
            }

            majorStopwatch.Stop();
        }

        public void IterationSetup()
        {
            Target = GenerateRandomTarget();

            if (Serializer is ProtobufSerializer)
                Target.CreateProtobufMessage();
            if (Serializer is ApacheThriftSerializer)
                Target.CreateThriftMessage();
            if (Serializer is ApacheAvroSerializer)
                Target.CreateAvroMessage();
            if (Serializer is CapnProtoSerializer)
                Target.CreateCapnProtoMessage();

            Target.Serialize(Serializer);
        }

        private ISerializationTarget GenerateRandomTarget()
        {
            var random = new Random().Next(4);
            return random switch
            {
                0 => new ChannelBuilder().Generate(),
                1 => new SocialInfoBuilder().Generate(),
                2 => new VideoInfoBuilder().Generate(),
                3 => new VideoBuilder().Generate(),
                _ => new ChannelBuilder().Generate(),
            };
        }
    }
}