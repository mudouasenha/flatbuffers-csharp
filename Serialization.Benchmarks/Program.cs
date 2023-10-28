using Microsoft.Extensions.Hosting;
using Serialization.Domain.Builders;
using Serialization.Serializers.FlatBuffers;
using Serialization.Services;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

Thread.Sleep(TimeSpan.FromSeconds(10));

//var config2 = new RESTSerializationBenchmarkConfig();
//BenchmarkRunner.Run<RESTSerializationBenchmark>(config2);

var host = builder.Build();

var myService = new WorkloadService();

await myService.RunParallelRestAsync(
    new FlatBuffersSerializer(),
    new ChannelBuilder().Generate().GetType(),
    24);

//await myService.Initialize();
//host.Run();