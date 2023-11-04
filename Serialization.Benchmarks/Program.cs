using BenchmarkDotNet.Running;
using Microsoft.Extensions.Hosting;
using Serialization.Benchmarks.Benchmarks;
using Serialization.Benchmarks.Configs;
using Serialization.Services;
using System.Net;

ServicePointManager.DefaultConnectionLimit = 5000;
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.Limits.MaxConcurrentConnections = 10000;
//});

builder.Services.AddServices();
Thread.Sleep(TimeSpan.FromSeconds(10));

//var config = new MultipleSerializationBenchmarkConfig();
//var config2 = new ConcurrencySerializationBenchmarkConfig();
var config3 = new RESTSerializationBenchmarkConfig();

//BenchmarkRunner.Run<MultipleSerializationBenchmark>(config);
//BenchmarkRunner.Run<ConcurrencySerializationBenchmark>(config2);
BenchmarkRunner.Run<RESTSerializationBenchmark>(config3);

//BenchmarkRunner.Run<MultipleSerializationBenchmark>(config);

var host = builder.Build();

//var ser = new ApacheAvroSerializer();
//var obj = new VideoInfoBuilder().Generate();
//ser.BenchmarkSerialize(obj.GetType(), obj);
//ser.BenchmarkDeserialize(obj.GetType(), obj);

//var myService = new WorkloadService();

//await myService.DispatchAsync(
//    new FlatBuffersSerializer(),
//    new ChannelBuilder().Generate().GetType().ToString(),
//    24);

//Console.WriteLine("Starting Processing");
//await myService.DispatchAsync(
//    new FlatBuffersSerializer(),
//    new ChannelBuilder().Generate().ToString(),
//    100);

//await myService.Initialize();
//host.Run();