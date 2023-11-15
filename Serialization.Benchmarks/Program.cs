using Serialization.Benchmarks;
using Serialization.Benchmarks.Configs;
using Serialization.Serializers.SystemTextJson;

//ServicePointManager.DefaultConnectionLimit = 5000;
//HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

//builder.Services.AddServices();
//Thread.Sleep(TimeSpan.FromSeconds(10));

var config3 = new RESTSerializationBenchmarkConfig();

//BenchmarkRunner.Run<RESTSerializationBenchmark>();

var benchmark = new RESTBenchmark();
var serializer = new NewtonsoftJsonSerializer();
await benchmark.Execute(serializer);

//var host = builder.Build();

//var myService = new WorkloadService();

//myService.GenerateAllRequests();