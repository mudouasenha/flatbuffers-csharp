using BenchmarkDotNet.Attributes;
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

namespace Serialization.Benchmarks.Benchmarks
{
    //[SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 0, iterationCount: 1, invocationCount: 1, id: "QuickJob")]
    [MinColumn, MaxColumn, AllStatisticsColumn, RankColumn] //, PerfCollectProfiler, EtwProfiler]
    public class RESTSerializationBenchmark
    {
        private RestClient client = new();
        private readonly HttpClient httpClient = new();
        private ExecutionInfo executionInfo;
        private static string BenchmarkDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        private const string fileName = "Measurements-REST-Client-SerializationBenchmark.csv";
        private readonly long epochTicks = new DateTime(1970, 1, 1).Ticks;

        private WorkloadService parallelService => new();

        [ParamsSource(nameof(Serializers))]
        public ISerializer Serializer { get; set; }

        //[ParamsSource(nameof(Targets))]
        public ISerializationTarget Target { get; set; }

        [Params(200, 500, 1000, 2000, 3000, 3500)]
        public int NumThreads { get; set; }

        public IEnumerable<ISerializer> Serializers => new ISerializer[]
        {
            //new FlatBuffersSerializer(),
            //new MessagePackCSharpSerializer(),
            //new NewtonsoftJsonSerializer(),
            ////new BinaryFormatterSerializer(),
            //new ProtobufSerializer(),
            //new ApacheThriftSerializer(),
            //new ApacheAvroSerializer(),
            new CapnProtoSerializer(),
        };

        public IEnumerable<ISerializationTarget> Targets => new ISerializationTarget[]
        {
        };

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

        [GlobalSetup]
        public void GlobalSetup()
        {
            //await parallelService.DispatchAsync(Serializer, null, NumThreads);
            executionInfo = new ExecutionInfo("Mixed", Serializer.ToString(), 0, NumThreads);
            Console.WriteLine("Aguardando a inicialização do próximo teste.");
            //Console.ReadKey();
            Console.WriteLine("Teste seguinte inicializado.");
        }

        //[IterationSetup(Target = nameof(SerializeAndDeserialize))]
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

        [Benchmark]
        public async Task Serialize()
        {
            var majorStopwatch = new Stopwatch();
            majorStopwatch.Start();

            while (majorStopwatch.Elapsed.TotalSeconds < 15)
            {
                IterationSetup();

                if (Serializer.GetSerializationResult(Target.GetType(), out var serializedTarget))
                {
                    var requestUrl = $"http://127.0.0.1:5020/receiver/serializer/serialize/{Target.ToString().ToLower()}"
                                .SetQueryParam("serializerType", Serializer.ToString())
                                .SetQueryParam("serializationType", Target.GetType().Name);

                    string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(Target);

                    var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                    {
                        Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                    };

                    var stopwatch = Stopwatch.StartNew();
                    var response = httpClient.SendAsync(request).GetAwaiter().GetResult();

                    stopwatch.Stop();

                    var result = new MeasurementRest(stopwatch.ElapsedMilliseconds, DateTimeOffset.Now.ToUnixTimeMilliseconds(), nameof(Serialize));

                    executionInfo.Measurements.Add(result);
                }

                // Wait for 1 second before the next iteration
                await Task.Delay(1000);
            }

            majorStopwatch.Stop();
        }

        [Benchmark]
        public async Task Deserialize()
        {
            var majorStopwatch = new Stopwatch();
            majorStopwatch.Start();

            while (majorStopwatch.Elapsed.TotalSeconds < 15)
            {
                IterationSetup();

                if (Serializer.GetSerializationResult(Target.GetType(), out var serializedTarget))
                {
                    var stopwatch = Stopwatch.StartNew();
                    var response = client.PostAsync("receiver/serializer/deserialize"
                                    .SetQueryParam("serializerType", Serializer.ToString())
                                    .SetQueryParam("serializationType", Target.GetType().Name), serializedTarget).GetAwaiter().GetResult();
                    stopwatch.Stop();
                    var result = new MeasurementRest(stopwatch.ElapsedMilliseconds, DateTimeOffset.Now.ToUnixTimeMilliseconds(), nameof(Deserialize));
                    executionInfo.Measurements.Add(result);
                }

                // Wait for 1 second before the next iteration
                await Task.Delay(1000);
            }

            majorStopwatch.Stop();
        }

        //public long Serialize() => Target.Serialize(Serializer);

        [GlobalCleanup]
        public async Task GlobalCleanup()
        {
            CsvExporter.ExportMeasurementsREST(fileName, executionInfo);
            //await parallelService.SaveServerResultsAsync(BenchmarkDateTime, Serializer.ToString(), Target.GetType().Name, NumThreads, null);
            Serializer.Cleanup();
            await Task.Delay(500);
        }
    }
}