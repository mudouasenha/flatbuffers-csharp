using BenchmarkDotNet.Running;
using Microsoft.Extensions.Hosting;
using Serialization.Benchmarks.Benchmarks;
using Serialization.Benchmarks.Configs;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

//Thread.Sleep(TimeSpan.FromSeconds(10));

var config = new MultipleSerializationBenchmarkConfig();
BenchmarkRunner.Run<MultipleSerializationBenchmark>(config);

var host = builder.Build();

//var ser = new ApacheAvroSerializer();
//var obj = new VideoInfoBuilder().Generate();
//ser.BenchmarkSerialize(obj.GetType(), obj);
//ser.BenchmarkDeserialize(obj.GetType(), obj);

//var myService = new WorkloadService();

//await myService.RunParallelRestAsync(
//    new FlatBuffersSerializer(),
//    new ChannelBuilder().Generate().GetType(),
//    24);

//await myService.Initialize();
//host.Run();