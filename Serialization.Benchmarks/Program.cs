using BenchmarkDotNet.Running;
using Microsoft.Extensions.Hosting;
using Serialization.Benchmarks.Benchmarks;
using Serialization.Benchmarks.Configs;
using Serialization.Services;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddServices();
Thread.Sleep(TimeSpan.FromSeconds(10));

var config = new RESTSerializationBenchmarkConfig();
BenchmarkRunner.Run<RESTSerializationBenchmark>(config);

//var host = builder.Build();

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